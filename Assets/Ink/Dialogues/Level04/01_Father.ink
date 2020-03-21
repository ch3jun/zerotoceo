﻿# father
# 999
INCLUDE ../Helper.ink
VAR minigames = 0

-> start

=== start ===
~ minigames = GetMinigameProgression()
~ xp = GetPlayerXP()
{
    - minigames == 2 && part_three:
        What are you still doing here? Get out there!
        -> END
    - minigames == 1 && part_two:
        What are you still doing here? Get out there!
        -> END
    - minigames == 0 && part_one:
        What are you still doing here? Get out there!
        -> END
    - minigames >= 3:
        -> part_four
    - minigames >= 2:
        -> part_three
    - minigames >= 1:
        -> part_two
    - else:
        -> part_one
}
= part_one
Enjoying the view?
    *   [Yeah.]
        Hah. It seems you are already acquainted with a life like this.
    *   [No.]
        You will learn to appreciate these kinds of luxuries some day.
- You seemed to have matured very much since I last saw you.
How long ago was that? Five weeks? Two days? Help me to recollect.
    *   [Um.]
        It's ok, it's ok. At least you've gained more confidence in your voice!
    *   [No.]
        Standing up to the big man! Very nice. You've also grown more confident in yourself.
- What did I tell you? Working here is an incredibly great opportunity.
- (question1) {As is being in that school club of yours. What was it called?|What was that school club of yours called?}
    *   [NJHS.]
        I was testing you, son; of course I know what kinds of clubs you're in. But do you yourself really not know?
        -> question1
    *   [FBLA.]
        Ah, yes. Future Business Leaders of America. Truly the dream of this country.
    *   [DECA.]
        I was testing you, son; of course I know what kinds of clubs you're in. But do you yourself really not know?
        -> question1
- Anyway, I want to see your improvement in action. Get down there and clean up that trash.
What are you waiting for? Go!
~ pendingMinigame = "Minigame_Trash_Hard"
-> END

= part_two
You've definitely become a hard worker. The exterior of this building is sparkly clean! Very nice job.
Now, I want to see your financial skills. We have more March of Dimes coins to sort. Get on it.
~ pendingMinigame = "Minigame_Coin_Medium"
-> END

= part_three
You're good with money. You're incredibly efficient as well. Who are you? Are you my son?
Haha, anyway... 
You see that grandma down there? She's trying to cross the street.
Help her out, and quickly! We don't want her getting run over.
You better clear those four flights of stairs hastefully.
~ pendingMinigame = "Minigame_Grandma_Hard"
-> END

= part_four
You... are a completely different person.
Again, I don't even know who you are anymore...
I don't want to tear up in front of you.
I think my time in this company is over. 
We need to discuss the future of BizTek in my office's back room.
~ xp += 100
Let's go. (Gained 100 XP)
-> END
