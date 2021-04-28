using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{

    public Discord.Discord discord;
    [SerializeField] string Details;
    [SerializeField] string State;
    [SerializeField] int StartTimestamp;
    
    // Start is called before the first frame update
    void Start()
    {
        discord = new Discord.Discord(836680203131551764, (System.UInt64)Discord.CreateFlags.Default);
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            Details = Details,
            State = State,
            Timestamps =
            {
                Start = StartTimestamp,
            },
        };
        activityManager.UpdateActivity(activity, (result) =>
        {
            if (result == Discord.Result.Ok)
            {
                Debug.Log("Successfully updated Discord Rich Presence");
            }
            else
            {
                Debug.LogError("Failed to update Discord Rich Presence");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        
        discord.Dispose();
    }
}
