#pragma strict

public var intro = new Array ();
public var off : float;
var WaitForNextScene = 4000;
public var text: String;
    
    function Start(){
     	WaitForNextScene = 4000;
		intro.length = 12;
		intro[11] = "In the 23rd century, the world witnessed its biggest disaster yet";
		intro[10] = "Peace was achieved thanks to a genetically-engineered being, Romeo";
		intro[9] = "He was engineered to lead humanity to peace and he singlehandedly achieved that goal through bloodshed and fear";
		intro[8] = "He was so powerful and cunning that humanity was enslaved by him and he grew stronger with each passing day";
		intro[7] = "There was no chance for humans to build a being to counter attack him, nor to stand up to him";
		intro[6] = "The self-proclaimed King of the world, Romeo made sure that humanity didn’t have access to technology and weapons to keep them under control";
		intro[5] = "Although this lead to the most peaceful era in humanity, people were unhappy of losing their freedom and to be terrorized by the evil King";
		intro[4] = "Some rebels managed to get across some old laboratory and create “Giuseppe, the liberator” whose purpose was to save humanity";
		intro[3] = "Giuseppe took it upon himself to travel to king Romeo's to achieve his goal";
		intro[2] = "Can this being defeat king Romeo and save the world?";
		intro[1] = "It’s up to you to help Giuseppe in his extremely difficult quest!";
		intro[0] = "Good Luck";
		
    }
    
    function OnGUI(){
    	WaitForNextScene = WaitForNextScene - Time.deltaTime;
	    off += Time.deltaTime * 4;
	    print (intro.length);
	    GUI.skin.label.fontSize = 12;
	    for (var i = 0; i < intro.length; i++){
	    	text = intro [i];
		    var roff = (intro.length-20) + (i*20 + off);
	    	var alph = Mathf.Sin((roff/Screen.height)*180*Mathf.Deg2Rad);
	    	GUI.color = new Color(1,1,1, alph);
	    	GUI.Label(new Rect(Screen.height/20,roff,Screen.width, 20),text);
	    	GUI.color = new Color(1,1,1,1);
	    	if(roff >= Screen.height + 20 * intro.length)
    		{
    			Application.LoadLevel("Level1");
    		}
		}
}