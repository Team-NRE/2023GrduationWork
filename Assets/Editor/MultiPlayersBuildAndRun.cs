using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiPlayersBuildAndRun
{
	// 툴 확장 기능 이용
	[MenuItem("Tools/Run Multiplayer/1 Player")]
	static void PerformWin64Build1()
	{
		PerformWin64Build(1);
	}

	// 툴 확장 기능 이용
	[MenuItem("Tools/Run Multiplayer/2 Player")]
	static void PerformWin64Build2()
	{
		PerformWin64Build(2);
	}

	[MenuItem("Tools/Run Multiplayer/3 Player")]
	static void PerformWin64Build3()
	{
		PerformWin64Build(3);

	}

	[MenuItem("Tools/Run Multiplayer/4 Player")]
	static void PerformWin64Build4()
	{
		PerformWin64Build(4);

	}

	// 빌드를 win64로 하라는 내용, 플레이어 이름과 프로젝트 이름으로 각기 다른 폴더에 만들어라
	static void PerformWin64Build(int playerCount)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

		for (int i = 1; i <= playerCount; i++)
		{
			BuildPipeline.BuildPlayer(GetScenePath(),
				"Build/Win64/0" + GetProjectName() + i.ToString() + "/" + GetProjectName() + i.ToString() + ".exe",
				BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer);
		}
	}

	// 프로젝트 이름을 받아온다.
	static string GetProjectName()
	{
		string[] s = Application.dataPath.Split('/');
		return s[s.Length - 2];
	}

	// 모든 씬을 가져오기위해 Scene의 경로를 가져온다.
	static string[] GetScenePath()
	{
		// EditorBuildSettigns : 빌드 창에 등록해둔 Scene들을 가져오는 클래스
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		return scenes;
	}
}
