using System.Text.Json;
using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Domain.Extensions;

public static class ElevenLabsResponseExtensions
{

    public static AlignmentDataModel ParseAlignment(this JsonElement source)
    {
        var alignment = new AlignmentDataModel
        {
            Charaters = [],
            CharacterStartTimesSeconds = [],
            CharacterEndTimesSeconds = []
        };

        if (source.TryGetProperty("characters", out var characters))
        {
            foreach (var character in characters.EnumerateArray())
            {
                var processedChar = character.GetString();
                if (string.IsNullOrEmpty(processedChar))
                    processedChar = " ";
                
                alignment.Charaters.Add(processedChar);
            }
        }
        
        if (source.TryGetProperty("character_start_times_seconds", out var startTimes))
        {
            foreach (var time in startTimes.EnumerateArray())
            {
                alignment.CharacterStartTimesSeconds.Add(time.GetDouble());
            }
        }
    
        if (source.TryGetProperty("character_end_times_seconds", out var endTimes))
        {
            foreach (var time in endTimes.EnumerateArray())
            {
                alignment.CharacterEndTimesSeconds.Add(time.GetDouble());
            }
        }
    
        return alignment;
    }


    public static byte[] CombineAudioChunks(this List<byte[]> audioChunks)
    {
        var total = audioChunks.Sum(x => x.Length);
        var combined = new byte[total];
        var offset = 0;

        foreach (var chunk in audioChunks)
        {
            Buffer.BlockCopy(chunk, 0, combined, offset, chunk.Length);
            offset += chunk.Length;
        }
        
        return combined;
    }
}