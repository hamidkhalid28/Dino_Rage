using UnityEngine;
using System.Collections;
using Gamelogic;
using System;
using Assets.SimpleAndroidNotifications;

public class LocalNotificationController : MonoBehaviour 
{
	public double Delay;

	public void sendNotification()
	{
//		if(Prefs.notificationTime == null)
//		{
//			NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(Delay), Constants.Game_Name,Constants.NotificationMessage, new Color(0, 0.6f, 1), NotificationIcon.Message);
//
//			Prefs.notificationTime = DateTime.Now.ToString();
//			Prefs.savePrefs();
//
//		}
//		else
//		{
//			DateTime notification_sent_time = DateTime.Parse(Prefs.notificationTime);
//			TimeSpan span = DateTime.Now.Subtract(notification_sent_time);
//
//			if(span.Days > 2)
//			{
//				NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(Delay), Constants.Game_Name,Constants.NotificationMessage, new Color(0, 0.6f, 1), NotificationIcon.Message);
//				Prefs.notificationTime = DateTime.Now.ToString();
//				Prefs.savePrefs();
//			}
//
//		}

//		NotificationManager.CancelAll ();
//
//		for(int i  = 1;i <= 2;i++)
//		{
//			NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(Delay * i), Constants.Game_Name,Constants.NotificationMessage, new Color(0, 0.6f, 1), NotificationIcon.Bell);
//		}

	}


}
