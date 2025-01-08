using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public class AdjacentGrid
    {
        private GridObject m_front = null;

        public GridObject Front
        {
            get { return m_front; }
            set { m_front = value; }
        }

        private GridObject m_back = null;

        public GridObject Back
        {
            get { return m_back; }
            set { m_back = value; }
        }

        private GridObject m_right = null;

        public GridObject Right
        {
            get { return m_right; }
            set { m_right = value; }
        }

        private GridObject m_left = null;

        public GridObject Left
        {
            get { return m_left; }
            set { m_left = value; }
        }
    }

    private struct Vector3_Structure
    {
        public Vector3 from;
        public Vector3 to;
    }

    public Quaternion Rotation
    {
        get { return transform.rotation; }
    }

    public Vector3 Normal
    {
        get { return Rotation * Vector3.up; }
    }

    public Vector3 Position
    {
        get { return transform.position; }
    }
    public delegate List<EncounterMicroCommander.EncounterCommand> GetEncounterCommands();

    [SerializeField, HideInInspector]
    private GetEncounterCommands m_encounterCommands = null;
    public GetEncounterCommands EncounterCommands
    {
        get { return m_encounterCommands; }
        set { m_encounterCommands = value; }
    }
    public delegate List<EventGridMicroCommander.EventGridCommand> GetEventCommands();
    [SerializeField, HideInInspector]
    private GetEventCommands m_eventCommands = null;
    public GetEventCommands EventCommands
    {
        get { return m_eventCommands; }
        set { m_eventCommands = value; }

    }

    private EncounterMicroCommander encounter = new();

    private EventGridMicroCommander eventGrid = new();

    [SerializeField, HideInInspector]
    private EventParameter eventStates;
    public EventParameter EvensStates
    {
        get { return eventStates; }
        set { eventStates = value; }
    }

    [SerializeField, HideInInspector]
    private EncounterParameter encounterParameter;
    public EncounterParameter EncounterStates
    {
        get { return encounterParameter; }
        set { encounterParameter = value; }
    }

    private AdjacentGrid adjacentGrid = new AdjacentGrid();

    public AdjacentGrid A_Grid
    {
        get { return adjacentGrid; }
    }

    private bool m_opened = false;

    public bool Open
    {
        get { return m_opened; }
        set { m_opened = value; }
    }

    [SerializeField, HideInInspector]
    private Vector2Int m_gridPoint;

    public Vector2Int GridPoint
    {
        set { m_gridPoint = value; }
        get { return m_gridPoint; }
    }

    private bool m_gridDataRender;

    public bool GridRender
    {
        set { m_gridDataRender = value; }
        get { return m_gridDataRender; }
    }

    [SerializeField, HideInInspector]
    private GridManager m_gridManager;

    public GridManager GetGridManager
    {
        get { return m_gridManager; }
    }

    public GridManager SetGridManager
    {
        set { m_gridManager = value; }
    }

    private int m_id = 0;

    public int Id
    {
        get { return m_id; }
        set { m_id = value; }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void OnDrawGizmos()
    {
        Vector3 vector3 = transform.position;
        Gizmos.color = (m_opened) ? new Color(1f, 0, 1f, 1f) : new Color(0f, 0, 1f, 1f);

        Vector3_Structure vector = GetVector3_Structure(Vector3.right + Vector3.back, Vector3.right + Vector3.forward);
        Gizmos.DrawLine(vector.from, vector.to);

        vector = GetVector3_Structure(Vector3.right + Vector3.back, Vector3.left + Vector3.back);
        Gizmos.DrawLine(vector.from, vector.to);

        vector = GetVector3_Structure(Vector3.left + Vector3.back, Vector3.left + Vector3.forward);
        Gizmos.DrawLine(vector.from, vector.to);

        vector = GetVector3_Structure(Vector3.left + Vector3.forward, Vector3.right + Vector3.forward);
        Gizmos.DrawLine(vector.from, vector.to);

        Gizmos.DrawLine(vector.from, vector.to);
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f);
        Gizmos.DrawRay(transform.position, Normal);
    }

    public EncounterMicroCommander OnEncounter()
    {
        if (m_encounterCommands == null) return null;
        var commands = m_encounterCommands();
        encounter.Clear();
        foreach (var command in commands)
        {
            encounter.AddCommand(command, encounterParameter);
        }
        return encounter;
    }

    public bool OnEncounter(out EncounterMicroCommander encounterMicroCommander)
    {
        encounterMicroCommander = null;
        if (m_encounterCommands == null) return false;
        var commands = m_encounterCommands();
        encounter.Clear();
        foreach (var command in commands)
        {
            encounter.AddCommand(command, encounterParameter);
        }
        encounterMicroCommander = encounter;
        return true;
    }


    public EventGridMicroCommander OnEvent()
    {
        if (m_eventCommands == null) return null;
        var commands = m_eventCommands();
        eventGrid.Clear();
        foreach (var command in commands)
        {
            eventGrid.AddCommand(command, eventStates);
        }
        return eventGrid;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void GridDown()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = (hit.point);
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    private Vector3_Structure GetVector3_Structure(Vector3 from, Vector3 to)
    {
        Quaternion quat = transform.rotation;
        Vector3 vector = transform.localScale;
        Vector3 position = transform.position;
        Vector3_Structure vector3_Structure = new Vector3_Structure();

        // ‘ã“ü
        vector3_Structure.from = from;
        vector3_Structure.to = to;

        // ‘å‚«‚³
        // from
        vector3_Structure.from.x *= vector.x;
        //vector3_Structure.from.y *= vector.y;
        vector3_Structure.from.z *= vector.z;
        // to
        vector3_Structure.to.x *= vector.x;
        //vector3_Structure.to.y *= vector.y;
        vector3_Structure.to.z *= vector.z;

        // Œü‚«
        vector3_Structure.from = quat * vector3_Structure.from;
        vector3_Structure.to = quat * vector3_Structure.to;

        // ˆÊ’u
        vector3_Structure.from += position;
        vector3_Structure.to += position;

        return vector3_Structure;
    }
}