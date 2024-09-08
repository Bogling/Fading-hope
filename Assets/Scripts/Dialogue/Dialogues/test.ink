EXTERNAL choiceMade(qID, cID)

This is test!
another line
-> main
=== main ===
Hello there? #color:d4821e
    + [Yes]
        ~ choiceMade(0, 0)
        -> chosen("Yes")
    + [No]
        ~ choiceMade(0, 1)
        -> chosen("No")
        
=== chosen(option) ===
You chose {option}! #color:ffffff
Start minigame1?
    + [Yes]
        ~ choiceMade(1, 0)
        -> mgs()
    + [No]
        ~ choiceMade(1, 1)
        -> mgc()
        
=== mgs() ===
Ok
-> DONE

=== mgc() ===
why
-> END