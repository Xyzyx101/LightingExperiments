using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookupTest : MonoBehaviour {
    [SerializeField]
    List<MonoBehaviour> Monos;
    [SerializeField]
    List<int> Ints;
    [SerializeField]
    List<string> Strings;

	void Awake () {
        Shuffle(ref Monos);
        Shuffle(ref Ints);
        Shuffle(ref Strings);
	}
	
	void Shuffle<T>(ref List<T> list) {
        for(int i = 0; i < 100; ++i ) {
            int a = Random.Range(0, list.Count);
            int b = Random.Range(0, list.Count);
            var tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }
    }

    private void Start() {
        int count = 10000000;

        var stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
        for(int i = 0; i < count; ++i ) {
            var x = GetMono();
            if(x != null) {
                int q = 1;
            }
        }
        Debug.Log("Monos time:" + stopWatch.ElapsedMilliseconds.ToString());
        stopWatch.Reset();

        stopWatch.Start();
        for( int i = 0; i < count; ++i ) {
            var x = GetInt();
            if( x == 0 ) {
                int q = 1;
            }
        }
        Debug.Log("int time:" + stopWatch.ElapsedMilliseconds.ToString());

        stopWatch.Reset();
        stopWatch.Start();
        for( int i = 0; i < count; ++i ) {
            var x = GetString();
            if( x == "" ) {
                int q = 1;
            }
        }
        Debug.Log("string time:" + stopWatch.ElapsedMilliseconds.ToString());
    }

    private MonoBehaviour GetMono() {
        var needle = Monos[Random.Range(0, Monos.Count)];
        return Monos.Find(haystack => haystack == needle);
    }
    private int GetInt() {
        var needle = Ints[Random.Range(0, Monos.Count)];
        return Ints.Find(haystack => haystack == needle);
    }
    private string GetString() {
        var needle = Strings[Random.Range(0, Monos.Count)];
        return Strings.Find(haystack => haystack == needle);
    }
}
