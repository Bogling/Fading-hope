EXTERNAL choiceMade(qID, cID)
Hello, nice to meet you again. #color:d4821e #image:icheerful
How do you feel?. #color:d4821e #image:iquestion
-> main
=== main ===
    + [I'm fine]
        -> fineans()
    + [Have been better]
        -> midans()
    + [Awful]
        -> awfans()

=== fineans() ===
Thats great! You are staying positive even in a situation like this.  #color:d4821e #image:ihappy
-> main1
=== midans() ===
I guess you are right... #color:d4821e #image:ithought
But I believe you can make it even though! #color:d4821e #image:icheerful
-> main1
=== awfans() ===
Oh... Probably it wasn't the best question for you... I am truly sorry #color:d4821e #image:isad
-> main1
=== main1 ===
Also... #color:d4821e #image:isad
I am sorry for tresspassing your personal space... #color:d4821e #image:ithought
I looked into your bio #color:d4821e #image:ithought
Your name is Nial, if you still don't remember, pretty nice name, isn't it? #color:d4821e #image:idefault
    + [Yeah, thanks]
        ~ choiceMade(0, 0)
        -> agreeans()
    + [How did you get my bio?]
        ~ choiceMade(0, 1)
        -> doubtans()
=== agreeans() ===
Oh, no need to thank me... #color:d4821e #image:iblush
I just want to do everything possible to help... that's all... #color:d4821e #image:iblush
-> main2
=== doubtans() ===
Oh... I... #color:d4821e #image:ithought
I just asked a nurse. Yeah. #color:d4821e #image:ifalsestate
-> main2
=== main2 ===
Sooo... #color:d4821e #image:idefault
Would you mind if we... #color:d4821e #image:idefault
Play a game? #color:d4821e #image:icheerful
I remember I promised you to bring something with me next time #color:d4821e #image:icheerful
    + [Yeah, why not]
        ~ choiceMade(1, 0)
        -> playans()
    + [I don't want to]
        ~ choiceMade(1, 1)
        -> denyans()

=== playans() ===
Oh, great! #color:d4821e #image:ihappy
Then, I guess we should start with something simple, aren't we? #color:d4821e #image:iquestion
Hmmm... #color:d4821e #image:ithought
Oh! #color:d4821e #image:iidea
Let's start with this. #color:d4821e #image:istate
I hide a coin in one hand, and then you guess which it is in. #color:d4821e #image:istate
Got it? Then let's start! #color:d4821e #image:icheerful
-> DONE
=== denyans() ===
Oh... I guess you are not in a mood for games today, isn't it? #color:d4821e #image:isad
I understand, but I will return tomorrow to check on you nonetheless. #color:d4821e #image:istate
-> DONE