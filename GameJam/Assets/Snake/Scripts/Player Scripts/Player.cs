using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum direction {Up,Right,Down,Left}
    [SerializeField]
    public Vector2Int Player_Position = new Vector2Int(0, 0);
    private direction Grid_Move_Direction = direction.Up;
    [SerializeField]
    private Audio AU;
    [SerializeField]
    public float GridMoveTimerMax=0.5f;
    private float GridMoveTimer = 0;
    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private GameObject Child_Prefab;
    [SerializeField]
    private Transform Child_Transform;
    private int Herd_Length = 0;
    public bool Game_Start=false;
    private List<ChickMovePosition> Chicks ;
    void Awake()
    {
        Chicks = new List<ChickMovePosition>();
    }
    void Update()
    {
        #region Movement
        if (Game_Start)
        {
            Movement();
            Auto();
        }
        #endregion
    }
    void OnCollisionEnter(Collision Object_Collider)
    {
        if(Object_Collider.collider.tag=="Wall"|| Object_Collider.collider.tag == "Chick")
        {
            GM.End();
        }
        if (Object_Collider.collider.tag == "Grass")
        {
            Destroy(Object_Collider.gameObject);
            AU.Play();
            GM.Score += 1;
            Herd_Length += 1;
            GM.CreateMeal();
        }
        if (Object_Collider.collider.tag == "Tomato")
        {
            Destroy(Object_Collider.gameObject);
            AU.Play();
            GM.Score += 2;
            Herd_Length += 2;
            GM.CreateMeal();
        }
        if (Object_Collider.collider.tag == "Cabbage")
        {
            Destroy(Object_Collider.gameObject);
            AU.Play();
            GM.Score += 5;
            Herd_Length += 3;
            GM.CreateMeal();
        }
    }
    void Movement()
    {
        #region Movement
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Grid_Move_Direction != direction.Down)
            {
                Grid_Move_Direction = direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Grid_Move_Direction != direction.Left)
            {
               Grid_Move_Direction = direction.Right;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Grid_Move_Direction != direction.Up)
            {
              Grid_Move_Direction = direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Grid_Move_Direction != direction.Right)
            {
                Grid_Move_Direction = direction.Left;
            }
        }
        #endregion
    }
    void Auto()
    {
        GridMoveTimer += Time.deltaTime;
        if (GridMoveTimer>=GridMoveTimerMax)
        {
            GridMoveTimer -= GridMoveTimerMax;
            ChickMovePosition Previous_CHMP = null;
            if (Chicks.Count>0)
            {
                Previous_CHMP = Chicks[0];
            }
            ChickMovePosition CHMP = new ChickMovePosition(Previous_CHMP,Player_Position, Grid_Move_Direction);
            Chicks.Insert(0, CHMP);

            Vector2Int Direction_Vector;
            switch(Grid_Move_Direction)
            {
                default:
                case direction.Up:
                    { Direction_Vector = new Vector2Int(0, 1); }
                    break;
                case direction.Right:
                    { Direction_Vector = new Vector2Int(1, 0); }
                    break;
                case direction.Down:
                    { Direction_Vector = new Vector2Int(0,-1); }
                    break;
                case direction.Left:
                    { Direction_Vector = new Vector2Int(-1, 0); }
                    break;
            }
            Player_Position += Direction_Vector;

            if(Chicks.Count>=Herd_Length+1)
            {
                Chicks.RemoveAt(Chicks.Count - 1);
            }
            for(int i=0;i< Chicks.Count;i++)
            {
                Vector2Int Player_Move_Position = Chicks[i].GetChickPosition();
                Vector3 Child_Position = new Vector3(Player_Move_Position.x, 1f, Player_Move_Position.y);
                GameObject Child = (GameObject)Instantiate(Child_Prefab, Child_Position, Child_Transform.rotation);
                Child.tag = "Chick";
                float angle;
                switch(Chicks[i].GetChickDirection())
                {
                    default:
                    case direction.Up:
                        {
                            switch (Chicks[i].GetPreviousDirection())
                            {
                                default:
                                    angle = 0;
                                    break;
                                case direction.Right:
                                    angle = 45;
                                    break;
                                case direction.Left:
                                    angle = -45;
                                    break;
                            }
                        }
                        break;
                    case direction.Right:
                        {
                            switch (Chicks[i].GetPreviousDirection())
                            {
                                default:
                                    angle = 90;
                                    break;
                                case direction.Up:
                                    angle = 45;
                                    break;
                                case direction.Down:
                                    angle = 135;
                                    break;
                            }
                        }
                        break;
                    case direction.Down:
                        {
                            switch (Chicks[i].GetPreviousDirection())
                            {
                                default:
                                    angle = 180;
                                    break;
                                case direction.Right:
                                    angle = 135;
                                    break;
                                case direction.Left:
                                    angle = -135;
                                    break;
                            }
                        }
                        break;
                    case direction.Left:
                        {
                            switch (Chicks[i].GetPreviousDirection())
                            {
                                default:
                                    angle = 270;
                                    break;
                                case direction.Up:
                                    angle = -45;
                                    break;
                                case direction.Down:
                                    angle = -135;
                                    break;
                            }
                        }
                        break;

                }
                Child.transform.eulerAngles = new Vector3(0, angle, 0);
                Destroy(Child, GridMoveTimerMax);
            }
            transform.position = new Vector3(Player_Position.x, 1, Player_Position.y);
            transform.eulerAngles = new Vector3(0, Angle(Direction_Vector), 0);
        }
    }
    private float Angle(Vector2Int Dir)
    {
        float Angle = Mathf.Atan2(Dir.x, Dir.y) * Mathf.Rad2Deg;
        return Angle;
    }
    private class ChickMovePosition
    {
        private ChickMovePosition Previous_Position;
        private Vector2Int Grid_Position;
        private direction Direction;
        public ChickMovePosition(ChickMovePosition Previus_Position,Vector2Int Grid_Position, direction Direction)
        {
            this.Previous_Position = Previus_Position;
            this.Grid_Position = Grid_Position;
            this.Direction = Direction;
        }
        public Vector2Int GetChickPosition()
        {
            return Grid_Position;
        }
        public direction GetChickDirection()
        {
            return Direction;
        }
        public direction GetPreviousDirection()
        {
            if (Previous_Position == null)
            {
                return direction.Up;
            }
            else
            { return Previous_Position.Direction; }
        }

    }
}

