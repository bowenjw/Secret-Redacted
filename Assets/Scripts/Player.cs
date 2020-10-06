using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isDead;
    private bool isPresident;
    private bool isChancellor;
    private bool wasChancellor;
    private bool isFascist;
    private bool isHitlar;
    private bool hasVoted;
    private bool votedYes;
    public Player(){
        isDead = true;
        isPresident = false;
        isChancellor = false;
        wasChancellor = false;
        isFascist = false;
        isHitlar = false;
        hasVoted = false;
        votedYes = false;
    }
    public bool IsDead { get{return isDead;} }
    public bool IsPresident { get{return isPresident;} }
    public bool IsChancellor { get{return isChancellor;} }
    public bool WasChancellor { get{return wasChancellor;} }
    public bool IsFascist { get{return isFascist;} }
    public bool IsHitlar { get{return isHitlar;} }
    public bool HasVoted { get{return hasVoted;} }
    public bool VotedYes { get{return votedYes;}}
    public void setHitlar() {
        isFascist = true;
        isHitlar = true;
    }
    public void setFascist(){
        isFascist = true;
    }
    public void died() {
        isDead = true;
    }
    public void president(){
        isPresident = true;
    }
    public void notPresident(){
        isPresident = false;
    }
    public void chancellor(){
        isChancellor = true;
    }
    public void notChancellor(){
        isChancellor = false;
        wasChancellor = true;
    }
    public void eligable(){
        wasChancellor = false;
    }
    public void yesVote(){
        votedYes = true;
        hasVoted = true;
    }
    public void noVote(){
        hasVoted = true;
    }
    public void RestVote(){
        votedYes = false;
        hasVoted = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
