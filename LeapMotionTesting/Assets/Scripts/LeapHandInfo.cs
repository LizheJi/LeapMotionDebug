using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class LeapHandInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftHandObject;
    [SerializeField]
    private GameObject _rightHandObject;

    [SerializeField]
    private Transform _leftLabel;
    [SerializeField]
    private Transform _rightLabel;
    [SerializeField]
    private List<Transform> _leftFingerLabel;
    [SerializeField]
    private List<Transform> _rightFingerLabel;

    [Header("Press Threshold:")]
    [SerializeField]
    private HandPairPressThreshold _thresholds;

    private HandModelBase _leftHand;
    private HandModelBase _rightHand;

    // Use this for initialization
    void Start()
    {
        _leftHand = _leftHandObject.GetComponent<HandModelBase>();
        _rightHand = _rightHandObject.GetComponent<HandModelBase>();
    }

    private void ToggleLeftHandLabels(bool isActivate)
    {
        _leftLabel.gameObject.SetActive(isActivate);
        foreach (Transform fingerLabel in _leftFingerLabel)
        {
            fingerLabel.gameObject.SetActive(isActivate);
        }
    }

    private void ToggleRightHandLabels(bool isActive)
    {
        _rightLabel.gameObject.SetActive(isActive);
        foreach (Transform fingerLabel in _rightFingerLabel)
        {
            fingerLabel.gameObject.SetActive(isActive);
        }
    }

    private string MakeHandRotationInfo(Hand hand, bool isRight)
    {
        string header = isRight ? "Right: " : "Left: ";
        StringBuilder sb = new StringBuilder();
        sb.Append(header);
        sb.AppendLine();

        float[] angleArray = FindFingersRotationFromHand(hand);
        int n = hand.Fingers.Count;
        for (int i = 0; i < n; i ++)
        {
            sb.Append(angleArray[i].ToString());
            sb.Append(", ");

        }
        return sb.ToString();
    }

    private float[] FindFingersRotationFromHand(Hand hand)
    {
        int n = hand.Fingers.Count;
        float[] angleArray = new float[n];
        for (int i = 0; i < n; i++)
        {
            angleArray[i] = FindFingerRotation(hand.Fingers[i]);
        }
        return angleArray;
    }

    private float FindFingerRotation(Finger finger)
    {
        Bone baseBone = finger.Bone(Bone.BoneType.TYPE_DISTAL);
        Bone midBone = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE);

        Quaternion baseBoneRotation = baseBone.Rotation.ToQuaternion();
        Quaternion midBoneRotation = midBone.Rotation.ToQuaternion();

        float angleDifference = Quaternion.Angle(midBoneRotation, baseBoneRotation);
        return angleDifference;
    }

    // Update is called once per frame
    void Update()
    {
        ToggleLeftHandLabels(_leftHand.IsTracked);
        ToggleRightHandLabels(_rightHand.IsTracked);

        if (_leftHand.IsTracked)
        {
            Hand leapHand = _leftHand.GetLeapHand();

            string handRotationInfo = MakeHandRotationInfo(leapHand, false);
            Debug.Log(handRotationInfo);
            _leftLabel.position = leapHand.PalmPosition.ToVector3();
            int n = leapHand.Fingers.Count;
            for (int i = 0; i < n; i++)
            {
                Finger leapFinger = leapHand.Fingers[i];
                _leftFingerLabel[i].position = leapFinger.TipPosition.ToVector3();
            }
            
        }
        if (_rightHand.IsTracked)
        {
            Hand leapHand = _rightHand.GetLeapHand();

            string handRotationInfo = MakeHandRotationInfo(leapHand, true);
            Debug.Log(handRotationInfo);

            _rightLabel.position = leapHand.PalmPosition.ToVector3();
            int n = leapHand.Fingers.Count;
            for (int i = 0; i < n; i++)
            {
                Finger leapFinger = leapHand.Fingers[i];
                _rightFingerLabel[i].position = leapFinger.TipPosition.ToVector3();
            }

        }
    }
}
