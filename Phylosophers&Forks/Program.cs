using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Phylosophers_Forks
{

    internal class Program
    {

        static int N = 5;
        static int[] state = new int[N];
        static Mutex[] forks = new Mutex[N]; //Створюю масив м'ютексів з виделок, кожен м'ютекс це якась виделка
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            for (int i = 0; i < N; i++)
            {
                forks[i] = new Mutex();
            }

            Thread[] threads = new Thread[N];//Створюю масив потоків, кожен з яких відповідає філософу
            for (int i = 0; i < N; i++)
            {
                threads[i] = new Thread(Philosopher);
                threads[i].Start(i);
            }

            Console.ReadLine();
        }

        static void Philosopher(object id)
        {
            int left = (int)id;
            int right = ((int)(id) + 1) % N;

            while (true)
            {
                //Стан виделок філософів 0 - якщо виделка вільна, 1 - якщо виделка зайнята, 2 - якщо виделка зайнята філософом
                state[left] = 0;
                state[right] = 0;

                //Якщо Філософ не їсть то він розмишляє
                Console.WriteLine("Філософ {0} розмишляє.", left);

                //Перевіряємо чи вілний м'ютекс для лівої виделки, якщо м'ютекс вільний то поток отримує його і продовжує роботу,якщо ні то поток блокується
                forks[left].WaitOne();
                state[left] = 1;

                //Тепер для правої
                forks[right].WaitOne();
                state[right] = 1;

                //Якщо все сходиться то Філософ приймає їжу
                Console.WriteLine("Філософ {0} приймає їжу.", left);

                Thread.Sleep(1000);

                //Звільнюю м'ютекси
                forks[left].ReleaseMutex();
                forks[right].ReleaseMutex();
            }
        }

    }
}
