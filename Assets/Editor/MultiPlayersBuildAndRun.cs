using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiPlayersBuildAndRun
{
	// �� Ȯ�� ��� �̿�
	[MenuItem("Tools/Run Multiplayer/1 Player")]
	static void PerformWin64Build1()
	{
		PerformWin64Build(1);
	}

	// �� Ȯ�� ��� �̿�
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

	// ���带 win64�� �϶�� ����, �÷��̾� �̸��� ������Ʈ �̸����� ���� �ٸ� ������ ������
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

	// ������Ʈ �̸��� �޾ƿ´�.
	static string GetProjectName()
	{
		string[] s = Application.dataPath.Split('/');
		return s[s.Length - 2];
	}

	// ��� ���� ������������ Scene�� ��θ� �����´�.
	static string[] GetScenePath()
	{
		// EditorBuildSettigns : ���� â�� ����ص� Scene���� �������� Ŭ����
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		return scenes;
	}
}
