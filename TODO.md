# TODOs

* Create a "relationship" table, to track relationships between people
    * For a census record, rather than putting the father (head of household) on a birth event, create a relationship entry instead
    * This will work better for something like an obituary, where it states "...survived by his sister Martha..."
    * It will also work better (I think) for the case where we look at two censuses, and can conclude Jane is the mother of Betty, because Jane was in census before Betty was born
* Rather than creating EVENT-TYPEs, EVENT-ROLE-TYPEs, and PLACE-TYPEs in an admin section, create those internally, as the code relies on them.
    * Use constants in the code, rather than magic strings scattered throughout the code and vault.
    * Replace "bride" and "groom" with "partner1" and "partner2"

