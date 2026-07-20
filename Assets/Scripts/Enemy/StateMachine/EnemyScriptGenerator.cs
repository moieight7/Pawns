#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Animations;
using static UnityEditor.VersionControl.Asset;

public class EnemyScriptGenerator : MonoBehaviour
{
    public string enemyName;
    public List<StateTemplate> states;

    public List<Type> stateList = new List<Type>();
    public List<string> stateNames;
    public List<Type> dataContainerList = new List<Type>();
    public List<string> dataContainers;

    [HideInInspector] public string enemyDataPath;
    [HideInInspector] public string enemySpecificFolder;
    [HideInInspector] public string combined;
    [HideInInspector] public GameObject template;

    public string stateFileFolder = "Assets/Scripts/Enemies/EnemySpecific";
    public string animationControllerFolder = "Assets/Animations/Controllers/Enemies";
    public int startingState = 0;
    public bool createGameObject = false, addEnemyComponentToGameObject = false, setComponentValues = false;

    void Start()
    {
        FindAllStates();
        FindAllDataContainerScripts();
    }

    public void FindAllStates()
    {
        stateList.Clear();
        stateNames.Clear();

        foreach (var type in TypeCache.GetTypesDerivedFrom(typeof(State)).Except(TypeCache.GetTypesDerivedFrom(typeof(State)).Where(t => TypeCache.GetTypesDerivedFrom(typeof(State)).Contains(t.BaseType))))
        {
            Debug.Log("Enemy Script Generator found state: " + type.FullName);
            stateList.Add(type);
            stateNames.Add(type.Name);
        }
    }

    public List<Type> FindAllDataContainerScripts()
    {
        dataContainerList.Clear();
        dataContainers.Clear();

        foreach (var type in TypeCache.GetTypesDerivedFrom(typeof(ScriptableObject)).Intersect(TypeCache.GetTypesDerivedFrom(typeof(IEnemyDataContainer))))
        {
            Debug.Log("Enemy Script Generator found data container: " + type.FullName);
            dataContainerList.Add(type);
            dataContainers.Add(type.Name);
        }

        return dataContainerList;
    }

    public void FindFolders()
    {
        if (!AssetDatabase.IsValidFolder(combined)) AssetDatabase.CreateFolder(enemySpecificFolder, enemyName);

        enemyDataPath = combined + "/" + "Data/";
        if (!AssetDatabase.IsValidFolder(enemyDataPath)) AssetDatabase.CreateFolder(combined, "Data");
    }

    public void CreateGameObject()
    {
        GameObject enemy = Instantiate(template, transform);
        enemy.name = enemyName;
        enemy.transform.parent = null;

        Animator animator = enemy.GetComponent<Animator>();

        AnimatorController animController = new AnimatorController();
        animController.name = enemyName;

        if (AssetDatabase.IsValidFolder(animationControllerFolder))
        {
            AssetDatabase.DeleteAsset(animationControllerFolder + "/" + enemyName + ".controller");
            AssetDatabase.CreateAsset(animController, animationControllerFolder + "/" + enemyName + ".controller");

            AnimatorControllerLayer layer = new AnimatorControllerLayer();
            layer.name = "Base Layer";
            layer.stateMachine = new AnimatorStateMachine();

            animController.AddLayer(layer);

            var stateMachine = layer.stateMachine;
            AnimatorState emptyState = stateMachine.AddState("Empty");
            List<AnimatorState> animStates = new List<AnimatorState>();
            animStates.Add(emptyState);
            List<AnimatorStateTransition> animTransitions = new List<AnimatorStateTransition>();
            
            foreach (StateTemplate state in states)
            {
                AnimatorState animState = stateMachine.AddState(enemyName.Replace(" ", "") + "_" + state.stateName);
                animController.AddParameter(state.animBoolName, AnimatorControllerParameterType.Bool);

                AnimatorStateTransition transitionToEmpty = animState.AddTransition(emptyState);
                transitionToEmpty.AddCondition(AnimatorConditionMode.IfNot, 0, state.animBoolName);
                animTransitions.Add(transitionToEmpty);

                AnimatorStateTransition transitionToState = emptyState.AddTransition(animState);
                transitionToState.AddCondition(AnimatorConditionMode.If, 0, state.animBoolName);
                animTransitions.Add(transitionToState);

                animStates.Add(animState);

                //AssetDatabase.AddObjectToAsset(animState, animationControllerFolder + "/" + enemyName + ".controller");
            }
            //AssetDatabase.AddObjectToAsset(emptyState, animationControllerFolder + "/" + enemyName + ".controller");
            foreach (AnimatorState state in animStates) AssetDatabase.AddObjectToAsset(state, animationControllerFolder + "/" + enemyName + ".controller");
            foreach (AnimatorStateTransition transition in animTransitions) AssetDatabase.AddObjectToAsset(transition, animationControllerFolder + "/" + enemyName + ".controller");

            stateMachine.defaultState = stateMachine.states[startingState + 1].state;

            AssetDatabase.AddObjectToAsset(stateMachine, animationControllerFolder + "/" + enemyName + ".controller");
            AssetDatabase.SaveAssets();

            animator.runtimeAnimatorController = animController;
        }
        else Debug.LogError("Enemy Script Generator failed to generate an animation controlller, " + animationControllerFolder + " is not a valid location.");

        Entity enemyEntity = null;
        foreach (var type in TypeCache.GetTypesDerivedFrom(typeof(Entity)))
        {
            Debug.Log("Enemy Script Generator found entity type: " + type.FullName);
            if (type.Name == enemyName)
            {
                enemyEntity = (Entity)enemy.AddComponent(type);
                break;
            }
        }
    }
}
#endif

#if UNITY_EDITOR
[System.Serializable]
public class StateTemplate
{
    public string stateName;
    public string stateToInherit;
    public string animBoolName;
    //public IEnemyDataContainer dataContainer;
}
#endif

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(EnemyScriptGenerator))]
public class EnemyScriptGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();
        EditorUtility.SetDirty(target);

        DrawDefaultInspector(); // for other non-HideInInspector fields

        EnemyScriptGenerator script = (EnemyScriptGenerator)target;

        GUILayout.Space(20);

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 15;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("~~Enemy Script Generator~~", titleStyle);
        //var script = (EnemyScriptGenerator)target;

        GUIStyle defaultStyle = new GUIStyle();
        defaultStyle.fontSize = 12;
        defaultStyle.alignment = TextAnchor.MiddleCenter;

        if (script.createGameObject) script.template = EditorGUILayout.ObjectField(new GUIContent("Template GameObject", "Template GameObject used for creating enemy gameobjects."), script.template, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Refresh"))
        {
            script.enemySpecificFolder = script.stateFileFolder;
            script.combined = script.enemySpecificFolder + "/" + script.enemyName;

            script.FindFolders();
            script.FindAllStates();
            script.FindAllDataContainerScripts();
        }

        if (GUILayout.Button("Generate Scripts"))
        {
            script.enemySpecificFolder = script.stateFileFolder;
            script.combined = script.enemySpecificFolder + "/" + script.enemyName;

            script.FindFolders();

            TextAsset stateTemplateTextTile = AssetDatabase.LoadAssetAtPath("Assets/Scripts/Enemy/StateMachine/Template/TransitionStateTemplate.txt",
                                                                   typeof(TextAsset)) as TextAsset;

            TextAsset stateDataTemplateTextTile = AssetDatabase.LoadAssetAtPath("Assets/Scripts/Enemy/StateMachine/Template/StateDataContainerTemplate.txt",
                                                                   typeof(TextAsset)) as TextAsset;

            TextAsset enemyTemplateTextTile = AssetDatabase.LoadAssetAtPath("Assets/Scripts/Enemy/StateMachine/Template/EnemyTemplate.txt",
                                                                   typeof(TextAsset)) as TextAsset;

            string contents = "";

            List<Type> dataContainers = new List<Type>();
            dataContainers = script.FindAllDataContainerScripts();
            Type DataContainer = null;
            ScriptableObject newContainer = null;

            foreach (StateTemplate state in script.states)
            {
                DataContainer = dataContainers.Find(t => t.Name.Remove(0, 2) == state.stateName);
                if (DataContainer != null)
                {
                    newContainer = CreateInstance(DataContainer);
                    AssetDatabase.CreateAsset(newContainer, script.enemyDataPath + script.enemyName + "_" + state.stateName + "Data" + ".asset");
                    AssetDatabase.SaveAssets();
                }
                else Debug.LogError("Cannot create new data container of type " + DataContainer.Name);
            }

            DataContainer = dataContainers.Find(t => t.Name == "D_Entity");
            if (DataContainer != null)
            {
                newContainer = CreateInstance(DataContainer);
                AssetDatabase.CreateAsset(newContainer, script.enemyDataPath + script.enemyName + "_" + "BaseData" + ".asset");
                AssetDatabase.SaveAssets();
            }
            else Debug.LogError("Cannot create new data container of type " + DataContainer.Name);

            if (stateTemplateTextTile != null)
            {
                contents = "";
                foreach (StateTemplate state in script.states)
                {
                    contents = stateTemplateTextTile.text;
                    contents = contents.Replace("STATE_NAME", script.enemyName + "_" + state.stateName.Replace(" ", ""));
                    contents = contents.Replace("PARENT_CLASS_NAME", state.stateToInherit.Replace(" ", ""));
                    contents = contents.Replace("ENEMY_CLASS_NAME", script.enemyName.Replace(" ", ""));
                    contents = contents.Replace("DATA_CONTAINER_CLASS_NAME", "D" + "_" + state.stateToInherit.Replace(" ", ""));

                    using (StreamWriter sw = new StreamWriter(string.Format(script.combined + "/{0}.cs", new object[] { script.enemyName + "_" + state.stateName.Replace(" ", "") })))
                    {
                        sw.Write(contents);
                    }

                    AssetDatabase.Refresh();
                }
            }
            else Debug.LogError("Can't find the TransitionStateTemplate.txt file! Is it at the path YOUR_PROJECT/Assets/Scripts/Enemies/EnemySpecific/TemplateEnemy/TransitionStateTemplate.txt?");

            if (enemyTemplateTextTile != null)
            {
                contents = enemyTemplateTextTile.text;
                contents = contents.Replace("ENEMY_NAME", script.enemyName.Replace(" ", ""));

                string stateReference = "";
                foreach (StateTemplate state in script.states)
                {
                    stateReference += "public STATE_TEMPLATE_NAME STATE_NAME { get; private set; }";
                    stateReference = stateReference.Replace("STATE_TEMPLATE_NAME", script.enemyName + "_" + state.stateName.Replace(" ", ""));
                    stateReference = stateReference.Replace("STATE_NAME", state.stateToInherit.Replace(" ", ""));
                    stateReference += "\n    ";
                }

                contents = contents.Replace("STATE_REFERENCES", stateReference);

                string stateDataContainerReference = "";
                foreach (StateTemplate state in script.states)
                {
                    stateDataContainerReference += "[SerializeField]\r\n    private DATA_CONTAINER_CLASS_NAME DATA_CONTAINER_NAME;";
                    stateDataContainerReference = stateDataContainerReference.Replace("DATA_CONTAINER_CLASS_NAME", "D" + "_" + state.stateToInherit.Replace(" ", ""));
                    stateDataContainerReference = stateDataContainerReference.Replace("DATA_CONTAINER_NAME", state.stateToInherit.Replace(" ", "") + "Data");
                    stateDataContainerReference += "\n    ";
                }

                contents = contents.Replace("STATE_DATA_CONTAINER_REFERENCES", stateDataContainerReference);

                string stateTransitionItemsReference = "";
                foreach (StateTemplate state in script.states)
                {
                    stateTransitionItemsReference += "STATE_NAME = new STATE_TEMPLATE_NAME(this, stateMachine, \"ANIM_BOOL_NAME\", DATA_CONTAINER_NAME, this);";
                    stateTransitionItemsReference = stateTransitionItemsReference.Replace("STATE_NAME", state.stateToInherit.Replace(" ", ""));
                    stateTransitionItemsReference = stateTransitionItemsReference.Replace("STATE_TEMPLATE_NAME", script.enemyName + "_" + state.stateName.Replace(" ", ""));
                    stateTransitionItemsReference = stateTransitionItemsReference.Replace("ANIM_BOOL_NAME", state.animBoolName.Replace(" ", ""));
                    stateTransitionItemsReference = stateTransitionItemsReference.Replace("DATA_CONTAINER_NAME", state.stateToInherit.Replace(" ", "") + "Data");
                    stateTransitionItemsReference += "\n        ";
                }

                contents = contents.Replace("STATE_TRANSITION_ITEMS_REFERENCES", stateTransitionItemsReference);
                contents = contents.Replace("STARTING_STATE", script.states[script.startingState].stateName);
            }
            else Debug.LogError("Can't find the EnemyTemplate.txt file! Is it at the path YOUR_PROJECT/Assets/Scripts/Enemies/EnemySpecific/TemplateEnemy/EnemyTemplate.txt?");

            //Let's create a new Script named "CHARACTERNAME.cs"
            using (StreamWriter sw = new StreamWriter(string.Format(script.combined + "/{0}.cs", new object[] { script.enemyName.Replace(" ", "") })))
            {
                sw.Write(contents);
            }
            //Refresh the Asset Database
            AssetDatabase.Refresh();

            if (script.createGameObject) script.CreateGameObject();
        }
    }
}
#endif

public interface IEnemyDataContainer { }