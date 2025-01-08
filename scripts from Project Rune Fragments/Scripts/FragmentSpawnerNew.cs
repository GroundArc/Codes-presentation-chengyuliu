using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentSpawnerNew : MonoBehaviour
{
    [System.Serializable]
    public class Fragment
    {
        public string name;
        public GameObject fragmentPrefab;
    }

    private float startDelay = 30f;
    private float delayBetweenSpawns = 60f;
    public List<Fragment> fragments = new List<Fragment>();
    public List<Transform> spawners = new List<Transform>();
    public List<GameObject> spawnedFragments = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnAllFragmentsWithInitialDelay());
    }

    private IEnumerator SpawnAllFragmentsWithInitialDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(SpawnAllFragments());
    }

    private IEnumerator SpawnAllFragments()
    {
        int spawnerIndex = 0;

        Fragment envyFragment = fragments.Find(f => f.name == "Envy");
        if (envyFragment != null)
        {
            Transform chosenSpawner = spawners[spawnerIndex];
            SpawnFragment(envyFragment, chosenSpawner);
            spawnerIndex++;
            fragments.Remove(envyFragment);
            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        while (fragments.Count > 0 && spawnerIndex < spawners.Count)
        {
            int fragmentIndex = Random.Range(0, fragments.Count);
            Fragment chosenFragment = fragments[fragmentIndex];
            Transform chosenSpawner = spawners[spawnerIndex];

            SpawnFragment(chosenFragment, chosenSpawner);

            fragments.RemoveAt(fragmentIndex);
            spawnerIndex++;

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    private void SpawnFragment(Fragment fragment, Transform spawner)
    {
        GameObject spawnedFragment = Instantiate(fragment.fragmentPrefab, spawner.position, spawner.rotation);
        spawnedFragments.Add(spawnedFragment);
        Debug.Log("Spawned " + fragment.name + " at " + spawner.transform.position + " at " + Time.time + " seconds");
    }
}