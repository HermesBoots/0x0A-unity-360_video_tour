using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CampusTour : MonoBehaviour
{
    public Camera view3d;
    public Camera viewUi;

    private float targetTime = 0;
    private Button target = null;
    private Dictionary<string, GameObject> rooms;
    private GameObject room;

#if UNITY_EDITOR
    private Vector2 mouse = new Vector2(-1, -1);
#endif


    private void Start() {
        uint index;
        MeshRenderer[] children;

        this.rooms = new Dictionary<string, GameObject>(this.transform.childCount);
        children = this.GetComponentsInChildren<MeshRenderer>(true);
        for (index = 0; index < children.Length; index++)
            this.rooms[children[index].tag] = children[index].gameObject;
        this.room = this.rooms["LivingRoom"];
    }


    private void Update()
    {
        bool result;
        RaycastHit cast;

#if UNITY_EDITOR
        this.RotateCamera();
#endif

        result = Physics.Raycast(
            this.view3d.transform.position,
            this.view3d.transform.forward,
            out cast,
            10f,
            ~LayerMask.NameToLayer("UI")
        );
        if (result) {
            if (this.target == null || !cast.collider.CompareTag(this.target.tag)) {
                this.target = cast.collider.GetComponent<Button>();
                this.target.image.color = new Color(.8f, .8f, .8f);
                this.targetTime = 0;
            }
            else
                this.targetTime += Time.deltaTime;
        }
        else {
            if (this.target != null) {
                if (this.target.CompareTag("Info"))
                    this.HideInfo(this.target);
                this.target.image.color = Color.white;
            }
            this.target = null;
        }
    }


    private void FixedUpdate() {
        if (this.target != null && this.targetTime > 0.5f) {
            if (this.target.CompareTag("Info"))
                this.ShowInfo(this.target);
            else
                this.MoveRoom(this.target);
        }
    }

    private void HideInfo(Button button) {
        button.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void ShowInfo(Button button) {
        button.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void MoveRoom(Button button) {
        this.room.SetActive(false);
        this.room = this.rooms[button.tag];
        button.image.color = Color.white;
        this.target = null;
        this.view3d.transform.position = this.room.transform.position;
        this.viewUi.transform.position = this.room.transform.position;
        this.room.SetActive(true);
    }



#if UNITY_EDITOR
    private void RotateCamera() {
        GameObject other;
        Vector2 angle;

        if (this.mouse.x >= 0) {
            angle.y = (Input.mousePosition.x - this.mouse.x) / Screen.width * 180;
            angle.x = (Input.mousePosition.y - this.mouse.y) / Screen.height * -180;
            this.view3d.transform.Rotate(new Vector3(0, angle.y), Space.World);
            this.view3d.transform.Rotate(new Vector3(angle.x, 0), Space.Self);
            other = GameObject.Find("UI Camera");
            other.transform.Rotate(new Vector3(0, angle.y), Space.World);
            other.transform.Rotate(new Vector3(angle.x, 0), Space.Self);
        }
        this.mouse = Input.mousePosition;
    }
#endif
}
