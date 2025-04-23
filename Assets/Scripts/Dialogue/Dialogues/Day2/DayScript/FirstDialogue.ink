EXTERNAL choiceMade(qID, cID)
Hello, nice to meet you again. #color:d4821e #image:icheerful #sound:0
How do you feel?. #color:d4821e #image:iquestion #sound:0
-> main
=== main ===
    + [I'm fine]
        -> fineans()
    + [Have been better]
        -> midans()
    + [Awful]
        -> awfans()

=== fineans() ===
Thats great! You are staying positive even in a situation like this.  #color:d4821e #image:ihappy #sound:0
-> main1
=== midans() ===
I guess you are right... #color:d4821e #image:ithought #sound:0
But I believe you can make it even though! #color:d4821e #image:icheerful #sound:0
-> main1
=== awfans() ===
Oh... Probably it wasn't the best question for you... I am truly sorry #color:d4821e #image:isad #sound:0
-> main1
=== main1 ===
Also... #color:d4821e #image:isad #sound:0
I am sorry for tresspassing your personal space... #color:d4821e #image:ithought #sound:0
I looked into your bio #color:d4821e #image:ithought #sound:0
Your name is Nial, if you still don't remember, pretty nice name, isn't it? #color:d4821e #image:idefault #sound:0
    + [Yeah, thanks]
        ~ choiceMade(0, 0)
        -> agreeans()
    + [How did you get my bio?]
        ~ choiceMade(0, 1)
        -> doubtans()
=== agreeans() ===
Oh, no need to thank me... #color:d4821e #image:iblush #sound:0
I just want to do everything possible to help... that's all... #color:d4821e #image:iblush #sound:0
-> main2
=== doubtans() ===
Oh... I... #color:d4821e #image:ithought #sound:0
I just asked a nurse. Yeah. #color:d4821e #image:ifalsestate #sound:0
-> main2
=== main2 ===
Sooo... #color:d4821e #image:idefault #sound:0
Would you mind if we... #color:d4821e #image:idefault #sound:0
Play a game? #color:d4821e #image:icheerful #sound:0
I remember I promised you to bring something with me next time #color:d4821e #image:icheerful #sound:0
    + [Yeah, why not]
        ~ choiceMade(1, 0)
        -> playans()
    + [I don't want to]
        ~ choiceMade(1, 1)
        -> denyans()

=== playans() ===
Oh, great! #color:d4821e #image:ihappy #sound:0
Then, I guess we should start with something simple, aren't we? #color:d4821e #image:iquestion #sound:0
Hmmm... #color:d4821e #image:ithought #sound:0
Oh! #color:d4821e #image:iidea #sound:0
Let's start with this. #color:d4821e #image:istate #sound:0
I hide a coin in one hand, and then you guess which it is in. #color:d4821e #image:istate #sound:0
Got it? Then let's start! #color:d4821e #image:icheerful #sound:0
-> DONE
=== denyans() ===
Oh... I guess you are not in a mood for games today, isn't it? #color:d4821e #image:isad #sound:0
I understand, but I will return tomorrow to check on you nonetheless. #color:d4821e #image:istate #sound:0
-> DONE