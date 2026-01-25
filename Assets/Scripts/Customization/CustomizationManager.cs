using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum AvatarPartType
{
    Hair,
    Face,
    Cloth,
    Glove,
    Shoe
}

[System.Serializable]
public class AvatarPartData
{
    public AvatarPartType partType;
    public List<SkinnedMeshRenderer> partSMRs = new();
}

public class EquippedPartInfo
{
    public int partIndex;
    public GameObject partGameObject;
}

public class CustomizationManager : Singleton<CustomizationManager>
{
    public GameObject originModelPrefab;
    public GameObject emptyModelPrefab;
    private GameObject _activeModel;
    public Transform spawnTransform;

    public List<AvatarPartData> avatarPartDataList = new();
    private Dictionary<string, Transform> _boneDictionary = new();
    private Dictionary<AvatarPartType, EquippedPartInfo> _currentEquippedPartDictionary = new();

    public Action<GameObject> OnCharacterCreated;

    // Start is called before the first frame update
    void Start()
    {
        // get all parts data
        GetAllPartSMRsDataFromOriginModel();

        //get bone data
        GetBoneDataFromActiveModel();

        // put on default parts data
        foreach (AvatarPartType type in System.Enum.GetValues(typeof(AvatarPartType)))
        {
            int indexToLoad = 0;
            if (AvatarCustomizationConfig.HasData)
            {
                indexToLoad = AvatarCustomizationConfig.GetPartIndex(type);
            }
            else
            {
                indexToLoad = 0;
            }

            ChangePart(type, indexToLoad);
        }
    }

    /// <summary>
    /// get all parts data
    /// </summary>
    void GetAllPartSMRsDataFromOriginModel()
    {
        if (originModelPrefab == null)
        {
            return;
        }

        GameObject originModel = Instantiate(originModelPrefab);
        originModel.name = "Origin_Parts_Database";
        originModel.SetActive(false);
        originModel.transform.SetParent(this.transform);

        avatarPartDataList.Clear();

        foreach (AvatarPartType partType in System.Enum.GetValues(typeof(AvatarPartType)))
        {
            AvatarPartData avatarPartData = new();
            avatarPartData.partType = partType;

            Transform typeTransform = originModel.transform.Find(partType.ToString());

            if (typeTransform != null)
            {
                var originSMRs = typeTransform.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                avatarPartData.partSMRs.AddRange(originSMRs);
            }
            else
            {
                Debug.LogWarning($"can not find object from {partType.ToString()}, check model");
            }

            avatarPartDataList.Add(avatarPartData);
        }
    }

    /// <summary>
    /// get bone data
    /// </summary>
    void GetBoneDataFromActiveModel()
    {
        if (_activeModel != null)
        {
            Destroy(_activeModel);
        }

        _activeModel = Instantiate(emptyModelPrefab);
        _activeModel.transform.position = spawnTransform == null ? Vector3.zero : spawnTransform.position;
        _activeModel.name = "ActiveModel";

        _boneDictionary.Clear();
        foreach (var bone in _activeModel.GetComponentsInChildren<Transform>())
        {
            if (!_boneDictionary.ContainsKey(bone.name))
            {
                _boneDictionary.Add(bone.name, bone);
            }
        }

        if (OnCharacterCreated != null)
        {
            OnCharacterCreated.Invoke(_activeModel);
        }
    }

    /// <summary>
    /// change parts
    /// </summary>
    /// <param name="partType"></param>
    /// <param name="index"></param>
    public void ChangePart(AvatarPartType partType, int index)
    {
        AvatarPartData tartgetAvatarPartData = avatarPartDataList.FirstOrDefault(g => g.partType == partType);

        if (tartgetAvatarPartData == null || index < 0 || index >= tartgetAvatarPartData.partSMRs.Count)
        {
            Debug.LogWarning($"can not find NO.{index} in {partType}");
            return;
        }

        SkinnedMeshRenderer targetPartSMRData = tartgetAvatarPartData.partSMRs[index];

        // delete old parts
        if (_currentEquippedPartDictionary.ContainsKey(partType))
        {
            var oldInfo = _currentEquippedPartDictionary[partType];
            if (oldInfo.partGameObject != null)
            {
                Destroy(oldInfo.partGameObject);
            }

            _currentEquippedPartDictionary.Remove(partType);
        }

        // put on new parts
        GameObject newPart = Instantiate(targetPartSMRData.gameObject);
        newPart.transform.SetParent(_activeModel.transform);

        SkinnedMeshRenderer newPartSMRData = newPart.GetComponent<SkinnedMeshRenderer>();

        // link new parts to active model bone
        Transform[] newBones = new Transform[targetPartSMRData.bones.Length];
        for (int i = 0; i < targetPartSMRData.bones.Length; i++)
        {
            string boneName = targetPartSMRData.bones[i].name;

            if (_boneDictionary.TryGetValue(boneName, out Transform targetBone))
            {
                newBones[i] = targetBone;
            }
            else
            {
                // if there is not a bone to link, use the default bone to avoid error
                newBones[i] = targetPartSMRData.bones[i];
            }
        }

        // link bone
        newPartSMRData.bones = newBones;

        if (targetPartSMRData.rootBone != null && _boneDictionary.ContainsKey(targetPartSMRData.rootBone.name))
        {
            newPartSMRData.rootBone = _boneDictionary[targetPartSMRData.rootBone.name];
        }

        // save data to dictionary
        EquippedPartInfo newPartInfo = new EquippedPartInfo();
        newPartInfo.partGameObject = newPart;
        newPartInfo.partIndex = index;
        _currentEquippedPartDictionary.Add(partType, newPartInfo);
    }

    /// <summary>
    /// play animation when change parts 
    /// </summary>
    /// <param name="partType"></param>
    public void PlayEquipAnimation(AvatarPartType partType)
    {
        if (_activeModel == null)
        {
            return;
        }

        Animator anim = _activeModel.GetComponent<Animator>();
        if (anim == null)
        {
            return;
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Base Layer.Idle"))
        {
            return;
        }

        switch (partType)
        {
            case AvatarPartType.Cloth:
                anim.SetTrigger("IsChangeClothes");
                break;

            case AvatarPartType.Shoe:
                anim.SetTrigger("IsChangeShoes");
                break;
        }
    }

    /// <summary>
    /// save current parts to static
    /// </summary>
    public void SaveCurrentPartsToStorage()
    {
        // find all parts currently
        foreach (var kvp in _currentEquippedPartDictionary)
        {
            AvatarPartType type = kvp.Key;
            int currentIndex = kvp.Value.partIndex;

            // save data to static
            AvatarCustomizationConfig.SavePart(type, currentIndex);
        }

        Debug.Log("all parts data has saved to static");
    }
}
