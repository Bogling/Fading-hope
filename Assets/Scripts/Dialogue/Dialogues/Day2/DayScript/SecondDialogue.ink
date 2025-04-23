EXTERNAL choiceMade(qID, cID)
Ok, let's stop with this game. #color:d4821e #image:idefault #sound:0
Hm... #color:d4821e #image:ithought #sound:0
Actually, it is raining today pretty much, isn't it? #color:d4821e #image:ithought #sound:0
By the way, do you like rainy weather? #color:d4821e #image:iquestion #sound:0
    + [I like it]
    ~ choiceMade(2, 0)
    -> like
    + [I don't]
    ~ choiceMade(2, 1)
    -> dislike
=== like ===
Oh, you like it? #color:d4821e #image:icheerful #sound:0
I like it too #color:d4821e #image:icheerful #sound:0
I find it so meditating #color:d4821e #image:idefault #sound:0
You know... those sounds and the whole atmosphere... #color:d4821e #image:idefault #sound:0
It is so calm... So you might for a moment forget about everything that bothers you... #color:d4821e #image:ithought #sound:0
And in a place I live, It does not rain at all... #color:d4821e #image:isad #sound:0
Oh, I got distracted, I am sorry #color:d4821e #image:idefault #sound:0
-> main2

=== dislike ===
Oh, I understand... #color:d4821e #image:idefault #sound:0
Sometimes they are problematic, if you need to be outside at the moment, right? #color:d4821e #image:ithought #sound:0
Maybe I like it because I can't see it very often... #color:d4821e #image:ithought #sound:0
Anyway it is quite strong, so I guess it is going to end soon. #color:d4821e #image:iwink #sound:0
-> main2

=== main2 ===
Oh, by the way, I told you that I read you bio... And I want to ask you... #color:d4821e #image:isad #sound:0
Do you actually remember, what happened to you? #color:d4821e #image:iquestion #wait:3 #sound:0
I guess not... #color:d4821e #image:isad #sound:0
Do you... want to know? I wont tell you if you don't want to hear. #color:d4821e #image:isadquestion #sound:0
    + [I want to know]
    ~ choiceMade(3, 0)
    -> face
    + [I don't want to know]
    ~ choiceMade(3, 1)
    -> escape

=== face ===
So... you were found somewhere in mountains... #color:d4821e #image:isad #sound:0
It seems like you fell through the snow into a cave... #color:d4821e #image:isad #sound:0
Your bio says... that the fall caused your brain critical damage... #color:d4821e #image:isad #sound:0
And now... your body is completly paralyzed except your right hand and the head... #color:d4821e #image:ifrightened #sound:0
But, even if the situation is this bad... #color:d4821e #image:isad #sound:0
You can make it! #color:d4821e #image:icheerful #sound:0
Doctors will find a way to help you, I am absolutely sure! #color:d4821e #image:istate #sound:0
And you are very strong, I know that. #color:d4821e #image:istate #sound:0
Despite your state you found strength to hear the truth. #color:d4821e #image:istate #sound:0
-> main3

=== escape ===
Oh, ok #color:d4821e #image:idefault #sound:0
Take it as slow as you need. #color:d4821e #image:idefault #sound:0
-> main3

=== main3 ===
By the way #color:d4821e #image:iidea #sound:0
We can utilize this coin a little more, aren't we? #color:d4821e #image:istate #sound:0
Let's do this... I toss this coin in the air, and after it lands you are guessing is it heads or tails, got it? #color:d4821e #image:idefault #sound:0
-> DONE