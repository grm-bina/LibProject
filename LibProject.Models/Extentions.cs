using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProject.Models
{
    public static class  Extentions
    {
            // Method to connect enum main-genres to his subgeneres
            public static List<Enum> GetSubgeners(this Genre genre)
            {
                string[] tempSubgeners;
                List<Enum> subgeners = new List<Enum>();

                switch (genre)
                {
                    case Genre.Fiction:
                        {
                            tempSubgeners = Enum.GetNames(typeof(Fiction));
                            for (int i = 0; i < tempSubgeners.Length; i++)
                            {
                                subgeners.Add((Fiction)Enum.Parse(typeof(Fiction), tempSubgeners[i]));
                            }
                            break;
                        }
                    case Genre.NonFiction:
                        {
                            tempSubgeners = Enum.GetNames(typeof(NonFiction));
                            for (int i = 0; i < tempSubgeners.Length; i++)
                            {
                                subgeners.Add((NonFiction)Enum.Parse(typeof(NonFiction), tempSubgeners[i]));
                            }
                            break;
                        }
                    case Genre.Academic:
                        {
                            tempSubgeners = Enum.GetNames(typeof(Academic));
                            for (int i = 0; i < tempSubgeners.Length; i++)
                            {
                                subgeners.Add((Academic)Enum.Parse(typeof(Academic), tempSubgeners[i]));
                            }
                            break;
                        }
                    default:
                        subgeners = null;
                        break;
                }



                return subgeners;
            }
    }

}
