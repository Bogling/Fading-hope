EXTERNAL choiceMade(qID, cID)
Should we stop for now? #color:d4821e #image:iwink
    + [Continue]
        ~ choiceMade(1, 0)
        -> DONE
    + [Stop]
        ~ choiceMade(1, 1)
        -> DONE