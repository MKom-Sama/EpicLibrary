using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicLibrary
{
    abstract class Member
    {
        public int ID;
        public string name;
        public string phoneNumber;

        public Member(int ID,string name,string phoneNumber)
        {
            this.ID = ID;
            this.name = name;
            this.phoneNumber = phoneNumber;
        }
    }

    class Student : Member
    {
        double discount = 0.1; // 10%

        public Student(int ID,string name,string phoneNumber)
            :base(ID,name,phoneNumber)
        {

        }
    }

    class Staff : Member
    {
        double discount = 0.25; // 25%

        public Staff(int ID, string name, string phoneNumber)
            : base(ID, name, phoneNumber)
        {

        }
    }

}
