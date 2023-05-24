using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] private LineRenderer _line;
    [SerializeField] private WhiteBall _whiteBall;
    [SerializeField] private List<NormalBall> _normalBalls;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private Menu _menu;

    public TMP_Text Text;
    protected int Hits = 0;
    private int index;
    // Use this for initialization
    void Start () {
        Text.text = "Hits: " + Hits;
    }
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var direction = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            var ballPos = new Vector3(_whiteBall.transform.position.x, 0.1f, _whiteBall.transform.position.z);
            var mousePos = new Vector3(hit.point.x, 0.1f, hit.point.z);
            _line.SetPosition(0, mousePos);
            _line.SetPosition(1, ballPos);
            direction = (mousePos - ballPos).normalized;
        }

        if (Input.GetMouseButtonUp(0) && _line.gameObject.activeSelf)
        {
            Hits++;
            Text.text = "Hits: " + Hits;
            _line.gameObject.SetActive(false);
            //_whiteBall.GetComponent<Rigidbody>().velocity = direction * 10f;
            _whiteBall.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
        }

        if (!_line.gameObject.activeSelf && _whiteBall.GetComponent<Rigidbody>().velocity.magnitude < 0.3f)
        {

            _line.gameObject.SetActive(true);
        }
        index = 0;
        for (int i = 0; i < _normalBalls.Count; i++)
        {
            if (!_normalBalls[i].gameObject.activeSelf)
            {
                index++;
            }
        }
        if (index == _normalBalls.Count)
        {
            _menu.OpenPanel(_winPanel);
        }
    }

    public void Reset()
    {
        _whiteBall.ResetBall();
        foreach (var ball in _normalBalls)
        {
            ball.gameObject.SetActive(true);
            ball.ResetBall();
        }
        Hits = 0;
    }

}
