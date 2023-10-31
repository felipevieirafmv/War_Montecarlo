using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

var timer = new Stopwatch();
int attackWin = 0;
int defenseWin = 0;
int quantAttack = 1_000;
int quantDefense = 500;
int quantRounds = 100_000;

montecarlo(quantRounds);

void Battle(int attack, int defense, Random rand)
{
    int currAttack, currDefense;
    int[] attackRoll = new int[3];
    int[] defenseRoll = new int[3];

    while (attack > 1 && defense > 0)
    {
        if (attack > 3)
            currAttack = 3;
        else if (attack > 2)
            currAttack = 2;
        else
            currAttack = 1;

        if (defense > 2)
            currDefense = 3;
        else if (defense > 1)
            currDefense = 2;
        else
            currDefense = 1;

        defense -= currDefense;
        attack -= currAttack;

        for (int i = 0; i < currAttack; i++)
            attackRoll[i] = rand.Next(6) + 1;

        for (int i = 0; i < currDefense; i++)
            defenseRoll[i] = rand.Next(6) + 1;

        Array.Sort(attackRoll);
        Array.Reverse(attackRoll);
        Array.Sort(defenseRoll);
        Array.Reverse(defenseRoll);

        for (int i = 0; i < 3; i++)
        {
            if (attackRoll[i] == 0 || defenseRoll[i] == 0)
                break;
            else if (attackRoll[i] > defenseRoll[i])
                currDefense--;
            else
                currAttack--;
        }

        attack += currAttack;
        defense += currDefense;
    }

    if (defense == 0)
        Interlocked.Increment(ref attackWin);
    else
        Interlocked.Increment(ref defenseWin);
}

void montecarlo(int round)
{
    timer.Start();

    Parallel.For(0, round, i =>
    {
        var rand = new Random();
        Battle(quantAttack, quantDefense, rand);
    });

    timer.Stop();

    TimeSpan timeTaken = timer.Elapsed;
    string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
    Console.WriteLine(foo);
    Console.WriteLine((float)attackWin / round * 100);
    Console.WriteLine((float)defenseWin / round * 100);
}
