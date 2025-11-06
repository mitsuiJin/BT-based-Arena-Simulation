using UnityEngine;

public enum NodeState { Running, Success, Failure }

public abstract class Node
{
    public abstract NodeState Evaluate();
}

//기초 노드 설정