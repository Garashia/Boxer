using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(5)]
public class MapMoving : MonoBehaviour
{
    [SerializeField]
    private GridManager m_gridManager;

    private GridObject m_grids = null;

    [SerializeField]
    private float m_higher;

    private int m_index = 0;

    [SerializeField]
    private List<UnityEngine.Events.UnityEvent> m_OnEncounter;

    private int m_encounterCount = 0;

    public int EncounterCount
    {
        get
        {
            return m_encounterCount;
        }
    }

    private System.Random m_random = new System.Random();

    [SerializeField]
    private Fade m_fade;

    private bool m_isMove = false;
    public bool IsMove
    {
        get { return m_isMove; }
        set { m_isMove = value; }
    }

    public GridObject Grid
    {
        set { m_grids = value; }
        get
        {
            m_grids ??= m_gridManager.FirstGrid;
            return m_grids;
        }
    }

    private const int ANGLE_MAX_INDEX = 4;

    private readonly float[] CAMERA_ANGLE = new float[ANGLE_MAX_INDEX]
        {0.0f, 90.0f, 180.0f, 270.0f};

    private enum DIRECT : uint
    {
        FRONT = 0,
        RIGHT = 1,
        BACK = 2,
        LEFT = 3,
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_encounterCount = m_OnEncounter.Count;
        m_isMove = false;
        m_index = 0;
        Vector3 vector3 = Vector3.zero;
        Grid = (GridSave.GridID > 0) ? m_gridManager.Grids[GridSave.GridID] : Grid;
        m_index = (GridSave.GridIndex > 0) ? GridSave.GridIndex : m_index;
        vector3.y = CAMERA_ANGLE[m_index];
        transform.rotation = Grid.Rotation * Quaternion.AngleAxis
        (vector3.y, Vector3.up);
        transform.position = Grid.transform.position + Grid.Rotation * (Vector3.up * m_higher);
        vector3.y = CAMERA_ANGLE[m_index];
        transform.rotation = Grid.Rotation * Quaternion.AngleAxis
        (vector3.y, Vector3.up);
        GetInputSystem.MapAction.Front.performed += Front;
        GetInputSystem.MapAction.RightRotation.performed += RightRotation;
        GetInputSystem.MapAction.LeftRotation.performed += LeftRotation;


    }

    // Update is called once per frame
    private void Update()
    {
        if (Grid == null)
            return;
    }

    public void RightRotation(InputAction.CallbackContext context)
    {
        if (m_isMove) return;
        Vector3 vector3 = Vector3.zero;
        m_index++;
        m_index %= ANGLE_MAX_INDEX;
        vector3.y = CAMERA_ANGLE[m_index];
        transform.rotation = Grid.Rotation * Quaternion.AngleAxis
        (vector3.y, Vector3.up);
    }

    public void LeftRotation(InputAction.CallbackContext context)
    {
        if (m_isMove) return;
        Vector3 vector3 = Vector3.zero;
        m_index = ANGLE_MAX_INDEX + m_index - 1;
        m_index %= ANGLE_MAX_INDEX;
        vector3.y = CAMERA_ANGLE[m_index];
        transform.rotation = Grid.Rotation * Quaternion.AngleAxis
        (vector3.y, Vector3.up);
    }

    public void Front(InputAction.CallbackContext context)
    {
        if (m_isMove) return;
        Vector3 vector3 = Vector3.zero;

        GridObject obj = null;
        if (m_index == (int)DIRECT.FRONT)
        {
            obj = Grid.A_Grid.Front;
        }
        else if (m_index == (int)DIRECT.BACK)
        {
            obj = Grid.A_Grid.Back;
        }
        else if (m_index == (int)DIRECT.RIGHT)
        {
            obj = Grid.A_Grid.Right;
        }
        else if (m_index == (int)DIRECT.LEFT)
        {
            obj = Grid.A_Grid.Left;
        }
        if (obj == null) return;

        Grid = obj;
        transform.position = Grid.transform.position + Grid.Rotation * (Vector3.up * m_higher);
        vector3.y = CAMERA_ANGLE[m_index];
        transform.rotation = Grid.Rotation * Quaternion.AngleAxis
        (vector3.y, Vector3.up);
        if (m_random.Probability(0.8f))
            m_OnEncounter[m_random.Next(m_encounterCount)].Invoke();
    }

    public void OnEncount()
    {
        return;
        //if (m_random.Probability(0.8f))
        //{
        //    isMove = true;
        //    GridSave.GridID = Grid.Id;
        //    GridSave.GridIndex = m_index;
        //    m_fade.FadeIn(1.0f, () =>
        //    {
        //        SceneManager.LoadSceneAsync("Player");
        //    });
        //    Debug.Log("Spawn");
        //}
    }
}