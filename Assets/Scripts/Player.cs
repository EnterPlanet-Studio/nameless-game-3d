using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private float _speed = 5f;
    private GameObject _arrow;
    private Transform _jumpChecker;
    private ParticleSystem _jumpParticle;
    [SerializeField] private bool _isGrounded = false;
    [SerializeField] private LayerMask whatIsGround;
    private int _jumpForce = 6;
    private float _rayDistance = 0.5f;
    [SerializeField] private Joystick joystick;
    [SerializeField] private TMP_Text timer, time, tutorial, trashLeft, _survive_time;
    private float fTime = 0f;
    private int iTime, iTimer = 0;
    [SerializeField] private float fTimer = -10f;
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _finishPanel;
    [SerializeField] private int _trash;
    private bool _finished = false;
    [SerializeField] private bool _isLevel = true;
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private GameObject _cat;
    [SerializeField] private AudioSource _safe, _notVerySafeAnymore, _youAreDead;
    private int _mus = 0;
    [SerializeField] private PostProcessVolume _postProc;
    ColorGrading _cg;
    [SerializeField]
    private Transform _cube;

    void Start()
    {
        if (_isLevel) {
            rb = gameObject.GetComponent<Rigidbody>();
            _arrow = transform.Find("arrow").gameObject;
            _jumpParticle = transform.Find("JumpParticle").gameObject.GetComponent<ParticleSystem>();
            _jumpChecker = transform.Find("JumpChecker");
            trashLeft.text = _trash.ToString();
            _postProc.profile.TryGetSettings(out _cg);

            if (fTimer == -666f) {
                InvokeRepeating("SpawnCat", 0f, 15f);
            }
        }
    }

    void Update() {
        if (!_finished && _isLevel) {
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                rb.linearVelocity = new Vector3(_speed*Input.GetAxis("Horizontal"), rb.linearVelocity.y, _speed*Input.GetAxis("Vertical"));
            else
                rb.linearVelocity = new Vector3(_speed*joystick.Horizontal, rb.linearVelocity.y, _speed*joystick.Vertical);
            _camera.position = new Vector3(transform.position.x + 3f, 6.5f, transform.position.z - 4f);

            if (rb.linearVelocity.x != 0 || rb.linearVelocity.z != 0) {
                Vector3 dir = new Vector3(-rb.linearVelocity.x, 0, -rb.linearVelocity.z);
                Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = rotation;
                _arrow.SetActive(true);
            }
            else _arrow.SetActive(false);

            if (Input.GetButtonDown("Jump")) Jump();

            fTime += Time.deltaTime;
            iTime = Mathf.RoundToInt(fTime);
            time.text = $"{iTime/60}:{iTime%60}";

            if (fTimer != -10f && fTimer != -666f && fTimer > 0f) {
                fTimer -= Time.deltaTime;
                iTimer = Mathf.RoundToInt(fTimer);
                timer.text = $"{iTimer/60}:{iTimer%60}";
            }
            else if (fTimer == -10f) timer.text = "Relax!";
            else if (fTimer == -666f) timer.text = "Survive!";
            else {
                timer.text = "RUN!!!";
                timer.color = Color.red;
                _cat.SetActive(true);
                _cg.active = true;
            }
            if (fTimer != -666f) {
                if (iTimer < 66 && iTimer != -10 && _mus < 1) {
                    _mus = 1;
                    _safe.Stop();
                    _notVerySafeAnymore.Play();
                } else if (iTimer <= 0 && iTimer != -10 && _mus < 2) {
                    _mus = 2;
                    _notVerySafeAnymore.Stop();
                    _youAreDead.Play();
                }
            }
        }
    }

    private void FixedUpdate() {
        if (_isLevel) {
            bool hit = Physics.Raycast(rb.position + new Vector3(0, 0.1f, 0), Vector3.down, _rayDistance, whatIsGround);

            _isGrounded = hit;
        }
    }

    void SpawnCat() {
        Instantiate(_cat, _cube.position, Quaternion.identity);
    }

    public void Jump() {
        if (_isGrounded) {
            _isGrounded = false;
            rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
            _jumpParticle.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door")) {
            //other.transform.Rotate(0, -85.0f, 0, Space.Self);
            other.gameObject.GetComponent<Animator>().SetTrigger("openDoor");
            other.tag = "Untagged";
        }
        else if (other.CompareTag("Finish") && _trash <= 0) {
            _finished = true;
            _finishPanel.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex > SaveManager.instance.activeSave.unlocked)
                SaveManager.instance.activeSave.unlocked = SceneManager.GetActiveScene().buildIndex;

            if (SaveManager.instance.activeSave.time.Count >= SceneManager.GetActiveScene().buildIndex) {
                if (SaveManager.instance.activeSave.time[SceneManager.GetActiveScene().buildIndex - 1] > iTime || SaveManager.instance.activeSave.time[SceneManager.GetActiveScene().buildIndex - 1] == 0)
                        SaveManager.instance.activeSave.time[SceneManager.GetActiveScene().buildIndex - 1] = iTime;
            } else
                SaveManager.instance.activeSave.time.Add(iTime);

            Time.timeScale = 0;
            SaveManager.instance.Save();
        }

        if (other.CompareTag("question")) {
            tutorial.text = other.name;
        }
    }
    void OnTriggerExit(Collider other)
    {
         if (other.CompareTag("question")) {
            tutorial.text = "";
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("garbage")) {
            _trash--;
            Destroy(other.gameObject);
            trashLeft.text = _trash.ToString();
        }
         else if (other.gameObject.CompareTag("enemy") && !_finished) {
            _deathPanel.SetActive(true);
            if (fTimer == -666f) {
                _survive_time.text = $"U survived: {iTime/60}:{iTime%60}";
                if (SaveManager.instance.activeSave.endless_time < iTime || SaveManager.instance.activeSave.endless_time == 0) {
                    SaveManager.instance.activeSave.endless_time = iTime; 
                    SaveManager.instance.Save();
                }
            }
            Time.timeScale = 0f;
         }
    }
}
