using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class UpgradeSpecLoader
{
    private const string CSV_ADDRESS = "UpgradeData";

    public static void LoadAsync(Action<List<UpgradeSpec>> onComplete)
    {
        Addressables.LoadAssetAsync<TextAsset>(CSV_ADDRESS).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var specs = Parse(handle.Result.text);
                onComplete?.Invoke(specs);
            }
            else
            {
                Debug.LogError($"[UpgradeSpecLoader] Failed to load: {CSV_ADDRESS}");
                onComplete?.Invoke(new List<UpgradeSpec>());
            }
        };
    }

    private static List<UpgradeSpec> Parse(string csvText)
    {
        var specs = new List<UpgradeSpec>();
        var lines = csvText.Split('\n');

        // 첫 줄(헤더) 스킵
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var spec = ParseLine(line);
            if (spec != null)
                specs.Add(spec);
        }

        return specs;
    }

    private static UpgradeSpec ParseLine(string line)
    {
        var values = line.Split(',');

        if (values.Length < 8)
        {
            Debug.LogWarning($"[UpgradeSpecLoader] Invalid line: {line}");
            return null;
        }

        if (!Enum.TryParse<UpgradeType>(values[0].Trim(), out var type))
        {
            Debug.LogWarning($"[UpgradeSpecLoader] Invalid UpgradeType: {values[0]}");
            return null;
        }

        return new UpgradeSpec
        {
            Type = type,
            Name = values[1].Trim(),
            Description = values[2].Trim(),
            MaxLevel = int.Parse(values[3].Trim()),
            BaseCost = float.Parse(values[4].Trim()),
            CostMultiplier = float.Parse(values[5].Trim()),
            BaseValue = float.Parse(values[6].Trim()),
            ValueMultiplier = float.Parse(values[7].Trim())
        };
    }
}
