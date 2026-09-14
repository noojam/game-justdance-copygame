using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Rendering.Universal.Internal;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;
using Windows.Kinect;

public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;
    public List<Vector2>[] BonePosList=new List<Vector2>[3];
    public Queue<List<Vector2>>[] BodyPosQueue = new Queue<List<Vector2>>[3];
    static public Transform[] BoneList;
     public Transform[] AvatarBoneList;
    public UnityEngine.AudioSource audioSource;
    public int[] score;
    public Slider[] scoreSlider;
    public float maxScore;
    public GameObject[] stars;
    public List<ulong> PlayertrackIds = new List<ulong>();
    public GameObject[][] PlayerStars =new GameObject[3][];
    public GameObject[] P1Stars;
    public GameObject[] P2Stars;
    public GameObject[] P3Stars;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    private List<Vector2> targetVec = new List<Vector2>();
    public float[] accuracy;

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
   
    private Dictionary<Kinect.JointType, Kinect.JointType> EvaluationTarget = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },

        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },

        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },


        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        //{ Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineMid, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
    private Dictionary<Transform, Transform> AvatarBone = new Dictionary<Transform, Transform>();
   
    public List<Image>[] resultList = new List<Image>[3];
    public List<Image> resultList1;
    public List<Image> resultList2;
    public List<Image> resultList3;
    public int[] currentState;
    public enum JointType2 : int
    {
        SpineBase = 12,
        SpineMid = 13,
        Neck = 14,
        Head = 15,
        ShoulderLeft = 8,
        ElbowLeft = 7,
        WristLeft = 6,
        ShoulderRight = 11,
        ElbowRight = 10,
        WristRight = 9,
        HipLeft = 2,
        KneeLeft = 1,
        AnkleLeft = 0,
        HipRight = 5,
        KneeRight = 4,
        AnkleRight = 3,
    }
    private void Awake()
    {
        PlayerStars[1] = P2Stars;
        PlayerStars[0] = P1Stars;
        PlayerStars[2] = P3Stars;
        accuracy= new float[3] { 0, 0, 0 };
        score = new int[3] { 0, 0, 0 };
        currentState = new int[3] { -1, -1, -1 };
        for (int i=0;i<BonePosList.Length;i++)
        {
            BonePosList[i] = new List<Vector2>();
        }
        for (int i = 0; i < BonePosList.Length; i++)
        {
            BodyPosQueue[i] = new Queue<List<Vector2>>();
        }
        resultList[0] = resultList1;
        resultList[1] = resultList2;
        resultList[2] = resultList3;
        BoneList = AvatarBoneList;
        AvatarBone = new Dictionary<Transform, Transform>()
    {
        { BoneList[0], BoneList[1]},
        { BoneList[1], BoneList[2] },

        { BoneList[3], BoneList[4]},
        { BoneList[4], BoneList[5] },

        { BoneList[6], BoneList[7]},
        { BoneList[7], BoneList[8] },

        { BoneList[9], BoneList[10]},
        { BoneList[10], BoneList[11] },

        { BoneList[12], BoneList[13] },
        { BoneList[13], BoneList[14] },
        { BoneList[14], BoneList[15] },
    };
    }

    void Update () 
    {                
            if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        PlayertrackIds = trackedIds;//[0];

        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
              }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
            
            if(body.IsTracked)
            {
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
                
                    RefreshBodyObject2(body, _Bodies[body.TrackingId]);

            }
        }
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);
            
            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }
        
        return body;
    }
    private void RefreshBodyObject2(Kinect.Body body, GameObject bodyObject)
    {
        for(JointType2 i = JointType2.AnkleLeft; i <= JointType2.Head;i++)
        {
            for(Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                if(HumanName1(i) == HumanName1(jt))
                {
                    Kinect.Joint sourceJoint = body.Joints[jt];
                    Kinect.Joint? targetJoint = null;
                    if (EvaluationTarget.ContainsKey(jt))
                    {
                        targetJoint = body.Joints[EvaluationTarget[jt]];
                    }
                    Transform jointObj = bodyObject.transform.Find(jt.ToString());
                    jointObj.position = GetVector3FromJoint(sourceJoint);
                    if (targetJoint.HasValue)
                    {
                        Vector2 sPoint = jointObj.position;
                        Vector2 ePoint = GetVector3FromJoint(targetJoint.Value);
                        Vector2 JointVec = new Vector2(ePoint.x - sPoint.x, ePoint.y - sPoint.y);
                        RePortJointVec(body,JointVec);
                    }
                }
            }
        }
    }
     string HumanName1( Enum @enum)
    {
        return @enum.ToString().Prettify();
    }
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            
            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);
            
            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            
            if(targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }
        }
    }
    void RePortJointVec(Kinect.Body body ,Vector2 add)
    {
        int index=-1;
        for(int i=0; i<PlayertrackIds.Count;i++)
        {
            if (PlayertrackIds[i]==body.TrackingId)
                index = i;
        }
        if (index>-1)
        {
            if (BonePosList[index]!=null)
            {
                if (BonePosList[index].Count == 11)
                {
                    List<Vector2> tempList = new List<Vector2>();
                    tempList = BonePosList[index].ToList();
                    RePortJointVecList(index, tempList);
                    BonePosList[index].Clear();
                }
            }
            BonePosList[index].Add(add);
        }
    }
    void RePortJointVecList(int index,List<Vector2> enqueue)
    {
        if (BodyPosQueue[index].Count>=90)
        {
            BodyPosQueue[index].Dequeue();
        }
        BodyPosQueue[index].Enqueue(enqueue);
    }
   
    private void EvaluateTargetVec()
    {       
        for (int k = 0; k < BoneList.Length; k++)
        {
            if (AvatarBone.ContainsKey(BoneList[k]))
            {
                targetVec.Add(new Vector2(AvatarBone[BoneList[k]].position.x - BoneList[k].position.x,
                    AvatarBone[BoneList[k]].position.y - BoneList[k].position.y));
            }
        }
    }
    public void apply()
    {
        
        for (int p=0;p<PlayertrackIds.Count;p++)
        {
            Queue<List<Vector2>> tempQueue = new Queue<List<Vector2>>();            
            tempQueue = BodyPosQueue[p];
            EvaluateTargetVec();
            List<List<Vector2>> tempList = QueueToList(tempQueue);
          
            List<float> accuracyList = new List<float>();
            for (int i = 0; i < tempList.Count; i++)
            {
                for (int j = 0; j < BonePosList[p].Count; j++)
                {
                    accuracy[p] += (targetVec[j].x * tempList[i][j].x + targetVec[j].y * tempList[i][j].y) /
                        (targetVec[j].magnitude * tempList[i][j].magnitude);
                }
                accuracyList.Add(accuracy[p]);
                accuracy[p] = 0;
            }
            targetVec.Clear();
            float accuracyMax = accuracyList.Max();
            if (accuracyMax >= 10.2f)
            {
                currentState[p] = 0;
                ShowResult(p);
                score[p] += 100;
                scoreSlider[p].value += 100;
                CheckingStars();
            }
            else if (accuracyMax >= 9.5f && accuracyMax < 10.2f)
            {
                currentState[p] = 1;
                ShowResult(p);
                score[p] += 80;
                scoreSlider[p].value += 80;
                CheckingStars();
            }
            else if (accuracyMax >= 8.5f && accuracyMax < 9.5f)
            {
                currentState[p] = 2;
                ShowResult(p);
                score[p] += 60;
                scoreSlider[p].value += 60;
                CheckingStars();
            }
            else if (accuracyMax < 8.5f)
            {
                currentState[p] = 3;
                ShowResult(p);
            }
            accuracyList.Clear();
        }
    }
    List<T> QueueToList<T>(Queue<T> queue)
    {
        return new List<T>(queue);
    }
    void ShowResult(int index)
    {
        for (int i = 0; i < resultList[index].Count; i++)
        {
                resultList[index][i].gameObject.SetActive(false);
        }
        resultList[index][currentState[index]].gameObject.SetActive(true);        
    }
    void CheckingStars()
    {
        for (int i = 0; i < PlayertrackIds.Count; i++)
        {
            if (score[i] > scoreSlider[i].maxValue / 6)
            {
                stars[0].SetActive(true);
                PlayerStars[i][0].SetActive(true);
            }
            if (score[i] > 2 * scoreSlider[i].maxValue / 6)
            {
                stars[1].SetActive(true);
                PlayerStars[i][1].SetActive(true);
            }
            if (score[i] > 3 * scoreSlider[i].maxValue / 6)
            {
                stars[2].SetActive(true);
                PlayerStars[i][2].SetActive(true);
            }
            if (score[i] > 4 * scoreSlider[i].maxValue / 6)
            {
                stars[3].SetActive(true);
                PlayerStars[i][3].SetActive(true);
            }
            if (score[i] > 5 * scoreSlider[i].maxValue / 6)
            {
                stars[4].SetActive(true);
                PlayerStars[i][4].SetActive(true);
            }
        }
    }
    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.green;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, (joint.Position.Y * 10)-100, joint.Position.Z * 10);
    }
}
