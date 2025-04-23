EXTERNAL choiceMade(qID, cID)
Oh, you woke up. #color:d4821e #image:isdefault #sound:0 
You look nervous, did you have a bad dream? #color:d4821e #image:isquestion #sound:0
    + [Yeah...]
        -> yes1
    + [No, I'm fine]
        -> lie1
    + [What are you doing on my bed?]
        -> q1
        
=== yes1 ===
~ choiceMade(1, 2)
Oh, that's... #color:d4821e #image:issad #sound:0
It is good you told me about it! #color:d4821e #image:ischeerful #sound:0
Some people say that if you tell somebody about your nightmare, tw will never become true #color:d4821e #image:isstate #sound:0
So I hope that is true #color:d4821e #image:isdefault #sound:0
->main1

== lie1 ===
~ choiceMade(1, 2)
Great #color:d4821e #image:ishappy #sound:0
I hope you are #color:d4821e #image:isdefault #sound:0
But... #color:d4821e #image:isthought #sound:0
Even if you have a nightmare, in the end, it is just a dream and nothing else #color:d4821e #image:isdefault #sound:0
So don't think about them too much #color:d4821e #image:iswink #sound:0
-> main1

=== q1 ===
~ choiceMade(1, 2)
I actually came earlier today #color:d4821e #image:isdefault #sound:0
And by the time I came, you were still sleeping #color:d4821e #image:isstate #sound:0
So I decided to wait here until you wake up... #color:d4821e #image:isdefault #sound:0
-> main1
=== main1 ===
By the way, it seems like nurses over here have some kind of a trouble #color:d4821e #image:isthought #sound:0
So I helped them and replaced that liquid thing by your side #color:d4821e #image:iswink #sound:0
Oh, and as I promised, I bringed something new today! #color:d4821e #image:isstate #sound:0
Some paper, to be exact #color:d4821e #image:isholding1 #sound:0
So we can play something more interesting #color:d4821e #image:ishappy #sound:0
So... Do you want to play something? #color:d4821e #image:isquestion #sound:0
    + [Yeah, why not]
        ~ choiceMade(0, 0)
        -> yes2
    + [I'm not in the mood to play now]
        ~ choiceMade(0, 1)
        -> no2

=== yes2 ===
Wonderful #color:d4821e #image:ishappy #sound:0
So, let's start with... #color:d4821e #image:isthought #sound:0
Oh, let's play Tic-Tac-Toe! #color:d4821e #image:isidea #sound:0
Give me a moment to draw you a grid... #color:d4821e #image:isholding2 #sound:0
Done! #color:d4821e #image:ischeerful #sound:0
-> DONE
=== no2 ===
Oh, I understand... #color:d4821e #image:issad #sound:0
Then I will leave you so you can rest #color:d4821e #image:isdefault #sound:0
But I will visit you again tomorrow #color:d4821e #image:isstate #sound:0
See you! #color:d4821e #image:isdefault #sound:0
-> DONE