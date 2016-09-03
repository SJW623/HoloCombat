using UnityEngine;

public class RobotAI : MonoBehaviour {

    public bool selected;
    // movement speed
    public int step;
    public float firerange;
    public GameObject enemytarget;
    public LineRenderer line;
    
    void Start() {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        if (selected) {
            transform.position = Vector3.MoveTowards(transform.position, WorldCursor.Instance.transform.position, step);
        }
        
        GameObject[] EnemyList = GameObject.FindGameObjectsWithTag("enemy");
        GameObject target = EnemyList[0];
        float mindist = Vector3.Distance(EnemyList[0].transform.position, transform.position);
        foreach (GameObject enemy in EnemyList) {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < mindist) {
                target = enemy;
                mindist = distance;
            }
        }

        if (mindist < firerange) {
            fire(target);
        }
        line.enabled = false;
    }
    
    void fire(GameObject target) {
        Ray ray = new Ray(transform.position, target.transform.position);
        RaycastHit hit;
        line.SetPosition(0, ray.origin);
        
        if (Physics.Raycast(ray, out hit, 100)) {
            line.SetPosition(1, hit.point);
            target.GetComponent<Attack_Robot>().targetted = true;
        } else {
            line.SetPosition(1, ray.GetPoint(100));
        }
        line.enabled = true;
    }
}