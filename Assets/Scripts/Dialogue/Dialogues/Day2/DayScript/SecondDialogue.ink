EXTERNAL choiceMade(qID, cID)
Ok, let's stop with this game. #color:d4821e #image:idefault
Hm... #color:d4821e #image:ithought
Actually, it is raining today pretty much, isn't it? #color:d4821e #image:ithought
By the way, do you like rainy weather? #color:d4821e #image:iquestion
    + [I like it]
    ~ choiceMade(2, 0)
    -> like
    + [I don't]
    ~ choiceMade(2, 1)
    -> dislike
=== like ===
Oh, you like it? #color:d4821e #image:icheerful
I like it too #color:d4821e #image:icheerful
I find it so meditating #color:d4821e #image:idefault
You know... those sounds and the whole atmosphere... #color:d4821e #image:idefault
It is so calm... So you might for a moment forget about everything that bothers you... #color:d4821e #image:ithought
And in a place I live, It does not rain at all... #color:d4821e #image:isad
Oh, I got distracted, I am sorry #color:d4821e #image:idefault
-> main2

=== dislike ===
Oh, I understand... #color:d4821e #image:idefault
Sometimes they are problematic, if you need to be outside at the moment, right? #color:d4821e #image:ithought
Maybe I like it because I can't see it very often... #color:d4821e #image:ithought
Anyway it is quite strong, so I guess it is going to end soon. #color:d4821e #image:iwink 
-> main2

=== main2 ===
Oh, by the way, I told you that I read you bio... And I want to ask you... #color:d4821e #image:isad
Do you actually remember, what happened to you? #color:d4821e #image:iquestion #wait:5
I guess not... #color:d4821e #image:isad
Do you... want to know? I wont tell you if you don't want to hear. #color:d4821e #image:isadquestion
    + [I want to know]
    ~ choiceMade(3, 0)
    -> face
    + [I don't want to know]
    ~ choiceMade(3, 1)
    -> escape

=== face ===
So... you were found somewhere in mountains... #color:d4821e #image:isad
It seems like you fell through the snow into a cave... #color:d4821e #image:isad
Your bio says... that the fall caused your brain critical damage... #color:d4821e #image:isad
And now... your body is completly paralyzed except your right hand and the head... #color:d4821e #image:ifrightened
But, even if the situation is this bad... #color:d4821e #image:isad
You can make it! #color:d4821e #image:icheerful
Doctors will find a way to help you, I am absolutely sure! #color:d4821e #image:istate
And you are very strong, I know that. #color:d4821e #image:istate
Despite your state you found strength to hear the truth. #color:d4821e #image:istate
-> main3

=== escape ===
Oh, ok #color:d4821e #image:idefault
Take it as slow as you need. #color:d4821e #image:idefault
-> main3

=== main3 ===
Bt the way #color:d4821e #image:iidea
We can utilize this coin a little more, aren't we? #color:d4821e #image:istate
Let's do this... I toss this coin in the air, and after it lands you are guessing is it heads or tails, got it? #color:d4821e #image:default
-> DONE