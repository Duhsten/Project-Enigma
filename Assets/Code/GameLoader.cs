using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    public List<LoadingTask> loaderTasks;
    public RawImage loaderbg;
    public RawImage loaderfg;
    public float maxWidth;
    public float currentWidth;
    public float incAmount;
    public int totalTasks;
    public int currentTask;
    // Start is called before the first frame update

    public void LoadTasks()
    {
        RegisterLoadingTask(new LoadingTask()
        {
            TaskName = "Data Check",
            Task = () => { ProjectEnigma.Data.LaunchManager.DataCheck(); },
        });
        RegisterLoadingTask(new LoadingTask()
        {
            TaskName = "Register Room Biomes",
            Task = () => { GameRegistry.RegisterRoomBiomes(); },
        });
        RegisterLoadingTask(new LoadingTask()
        {
            TaskName = "Register Cards",
            Task = () => { GameRegistry.LoadCardPacks(); },
        });

    }
    public void RegisterLoadingTask(LoadingTask action)
    {
        loaderTasks.Add(action);
    }


    public void StartLoader()
    {
        for(int i = 0; i < loaderTasks.Count; i++)
        {
            StartCoroutine(RunLoadedTask(loaderTasks[i], returnValue =>
            {
                if (!returnValue.Item1)
                {
                    Debug.LogError($"Error: {returnValue.Item2}");
                    return;
                   
                }

            }
        ));
        }
    }
    private IEnumerator RunLoadedTask(LoadingTask action, System.Action<Tuple<bool,string>> callback = null)
    {
        var result = action.RunTask();
        if(result.Item1)
        {
            currentTask++;
            UpdateProgressBar();
            yield return true;
            if (callback != null) { callback.Invoke(new Tuple<bool, string>(true, $"Task: {action.TaskName} completed.")); }
            
        }
        else
        {
            yield return false;
            if (callback != null) { callback.Invoke(new Tuple<bool, string>(false, $"Task: {action.TaskName} failed.")); }
        }
        
    }
   
    void Start()
    {
        loaderTasks = new List<LoadingTask>();
        LoadTasks();
        totalTasks = loaderTasks.Count;
        maxWidth = loaderbg.GetComponent<RectTransform>().rect.size.x;
        incAmount = maxWidth  / totalTasks;
        StartLoader();
    }
    public void UpdateProgressBar()
    {
        loaderfg.GetComponent<RectTransform>().sizeDelta = new Vector2((currentTask * incAmount), loaderfg.GetComponent<RectTransform>().sizeDelta.y);
    }
    // Update is called once per frame
    void Update()
    {
        if(currentTask >= totalTasks)
        {
            SceneManager.LoadScene(1);
        }
    }
    public class LoadingTask
    {
        public System.Action Task;
        public string TaskName;
        public Tuple<bool,string> RunTask()
        {
            try
            {
                Task.Invoke();
                return new Tuple<bool, string>(true, $"Task: {TaskName} completed.");
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, $"Task: {TaskName} failed.");
            }
        }
    }
}
