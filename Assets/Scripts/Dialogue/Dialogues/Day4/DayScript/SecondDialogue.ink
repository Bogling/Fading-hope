EXTERNAL choiceMade(qID, cID)
~ choiceMade(0, 0)
Oh, it seems like you woke up earlier today #color:d4821e #image:isdefault #sound:0
How are you today? #color:d4821e #image:isquestion #sound:0
    + [I'm fine]
        ~ choiceMade(1, 0)
        -> fine1
    + [Something's off...]
        ~ choiceMade(1, 1)
        -> off1
    + [Who were you talking to?]
        ~ choiceMade(1, 2)
        -> q1

=== fine1 ===
Oh, I'm glad you feel alright! #color:d4821e #image:ishappy #sound:0
You know, there are not so many people who can be this positive even in their daily lives #color:d4821e #image:ischeerful #sound:0
Keep up with this attidude and your illnes won't stand a chance! #color:d4821e #image:isstate #sound:0
-> main1

=== off1 ===
Oh, can it be... #color:d4821e #image:issad #sound:0
Just don't worry #color:d4821e #image:ischeerful #sound:0
I know you are going to be fine. #color:d4821e #image:isstate #sound:0
I... #color:d4821e #image:issad #sound:0
I promise! #color:d4821e #image:ischeerful #sound:0
-> main1

=== q1 ===
Hm...? #color:d4821e #image:isdefault #sound:0
I haven't talked to anybody. #color:d4821e #image:isfalsestate #sound:0
Maybe you weren't fully awake yet? #color:d4821e #image:isfalsestate #sound:0
-> main1

=== main1 ===
And by the way... #color:d4821e #image:isdefault #sound:0
Today I brought... cards! #color:d4821e #image:isstate #sound:0
I already have some ideas on what we can play in with them #color:d4821e #image:iswink #sound:0
What do you think, do you want to play today? #color:d4821e #image:isquestion #sound:0
    + [Yes, I'm in]
        ~choiceMade(2,0)
        -> yes2
    + [No, I'll pass]
        ~choiceMade(2,1)
        -> no2

=== yes2 ===
Splendit! #color:d4821e #image:ishappy #sound:0
Then let us start with something like... #color:d4821e #image:isthought #sound:0
Let's call it match two. #color:d4821e #image:isdefault #sound:0
I place cards randomly and then you find the ones that have equal value #color:d4821e #image:isstate #sound:0
Are you ready? #color:d4821e #image:isdefault #sound:0
Then #color:d4821e #image:iswink #sound:0
-> DONE

=== no2 ===
Okay, then... #color:d4821e #image:issad #sound:0
I will leave you for now #color:d4821e #image:isdefault #sound:0
But don't forget that tomorrow I will return! #color:d4821e #image:ischeerful #sound:0
You won't be alone here... #color:d4821e #image:ischeerful #sound:0
So... Goodbye! #color:d4821e #image:isdefault #sound:0
-> DONE