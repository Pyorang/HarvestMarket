#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
#endif
using System;

[Serializable]
#if !UNITY_WEBGL || UNITY_EDITOR
[FirestoreData]
#endif
public class Dog
{
#if !UNITY_WEBGL || UNITY_EDITOR
    [FirestoreDocumentId]
#endif
    public string Id { get; set; }

#if !UNITY_WEBGL || UNITY_EDITOR
    [FirestoreProperty]
#endif
    public string Name { get; set; }

#if !UNITY_WEBGL || UNITY_EDITOR
    [FirestoreProperty]
#endif
    public int Age { get; set; }

    public Dog() { }

    public Dog(string name, int age)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new System.ArgumentNullException("이름은 비어있을 수 없습니다.");
        }

        if (age <= 0)
        {
            throw new System.ArgumentNullException("나이는 0살보다 작을 수 업습니다.");
        }

        Name = name;
        Age = age;
    }
}
