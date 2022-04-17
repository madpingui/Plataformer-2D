using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveForce, maxSpeed, jumpForce, dashForce;
    private float Player_xAxis, dash_side;
    private bool grounded, dashing, doubleJump, jumping, looking_front = true;
    [Tooltip("attach here an invisible gameObjetc that has to be above the player always, this checks if the player collides with the floor")]
    public Transform groundCheck;
    private Rigidbody2D PlayerRb;
    private Animator PlayerAnim;
    private SpriteRenderer PlayerSprite;

    void Awake()
    {
        PlayerRb = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponent<Animator>();
        PlayerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Water"));
        if (grounded && jumping)
        {
            landing();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Atacar();
        }
            
        CanSpriteFlip();
    }

    void FixedUpdate()
    {
        Player_xAxis = Input.GetAxis("Horizontal");
        PlayerAnim.SetFloat("Speed", Mathf.Abs(Player_xAxis));
        
        if (Player_xAxis != 0)
        {
            Correr();
            if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing && !jumping)
                Sprint();
        }

        if(grounded && Input.GetKeyDown(KeyCode.Space))
            Saltar();
        else if(doubleJump && Input.GetKeyDown(KeyCode.Space))
            SaltoDoble();
    }

    #region Movement

    void CanSpriteFlip(){
        if ( looking_front && (Player_xAxis < 0))
        {
            StopDashing();
            PlayerSprite.flipX = true;
            looking_front = false;
        }
        else if ( !looking_front && (Player_xAxis > 0))
        {
            StopDashing();
            PlayerSprite.flipX = false;
            looking_front = true;
        }
    }

    public void Correr(){
        if (Player_xAxis * PlayerRb.velocity.x < maxSpeed) 
            PlayerRb.AddForce(new Vector2(Player_xAxis * moveForce, 0f));
        if ( (Mathf.Abs(PlayerRb.velocity.x) > maxSpeed) && !dashing)
            PlayerRb.velocity = new Vector2(Mathf.Sign(PlayerRb.velocity.x) * maxSpeed, PlayerRb.velocity.y);
    }

    public void Sprint() //called again at the end of the animation for desactivation
    {
        dashing = !dashing;
        PlayerAnim.SetBool("dash", dashing);
        if(dashing)
        {
            dash_side = (Player_xAxis > 0)? 1 : -1;
            PlayerRb.AddForce(new Vector2(dashForce * dash_side, 0f));
        }
    }

    void StopDashing(){
        dashing = false;
        PlayerAnim.SetBool("dash", false);
    }
    #endregion

    #region Jump

    void landing(){
        doubleJump = false;
        jumping = false;
        PlayerAnim.SetBool("jumpAttack", false);
        PlayerAnim.SetBool("jump", false);
    }

    public void Saltar(){
        doubleJump = true;
        grounded = false;
        jumping = true;
        StopDashing();
        PlayerRb.AddForce(new Vector2(0f, jumpForce));
        PlayerAnim.SetBool("jump", true);
    }

    public void SaltoDoble(){
        PlayerAnim.SetBool("jump", true); 
        PlayerRb.velocity = new Vector2(PlayerRb.velocity.x, 0); 
        PlayerRb.AddForce(new Vector2(0f, jumpForce));
        doubleJump = false;
    }

    void ReturnToJump(){ //called at the and of the player jumping animations
        StopDashing();
        PlayerAnim.SetBool("jumpAttack", false);
        PlayerAnim.SetBool("jump", true);
        jumping = true;
    }
    #endregion

    #region Actions

    public void Atacar(){ //at the end of tis animation we call stopDash so he still dashing while attacking
        if (grounded){
            PlayerAnim.SetBool("dash", false);
            PlayerAnim.SetTrigger("basicAttack");
        }
            
        else{
            PlayerAnim.SetBool("jump", false);
            PlayerAnim.SetBool("jumpAttack", true);
        }
    }
    #endregion

    #region Collisions

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "mapLimits"){
            CameraController.EnterToMapLimits = true;
        }
        else if(other.gameObject.tag == "InstaDeath"){
            GameManager.ReloadGame();
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if( (other.gameObject.tag == "Enemy") && !GameManager.beingImmune){
            GameManager.PlayerTakeDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D mapLimits){
        if(mapLimits.gameObject.tag == "mapLimits"){
            CameraController.EnterToMapLimits = false;
        }
    }
    #endregion
}