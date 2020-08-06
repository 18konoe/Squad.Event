using SquadEvent.Shared.Models;
using System;
using System.Collections.Generic;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Test
{
    public class EventTestData
    {
        public Sha256HashString TestPasswordProvider { get; }
        public List<EventModel> AllTestData { get; } = new List<EventModel>();
        public EventModel JustNewEvent { get; }
        public EventModel InitialFillEvent { get; }
        public EventModel BasicDraftEvent1 { get; }
        public EventModel BasicDraftEvent2 { get; }
        public EventModel BasicOpenEvent1 { get; }
        public EventModel BasicOpenEvent2 { get; }
        public EventModel BasicFixedEvent { get; }
        public EventModel EditorExistEvent1 { get; }
        public EventModel EditorExistEvent1P { get; }
        public EventModel EditorExistEvent2 { get; }
        public EventModel EditorExistEvent3 { get; }
        public EventModel UpdatedEvent { get; }
        public EventModel UpdateStateTarget1 { get; }
        public EventModel UpdateStateTarget1P { get; }
        public EventModel FilterScheduleTest1 { get; }
        public EventModel FilterScheduleTest2 { get; }
        public EventModel FilterScheduleTest3 { get; }
        public EventModel FilterScheduleTest4 { get; }
        public EventModel FilterScheduleTest5 { get; }
        private readonly DateTimeOffset _today;

        private Random _rand;
        public Dictionary<string, string> OriginatorIdDictionary = new Dictionary<string, string>();
        public EventTestData(int seed, DateTimeOffset today)
        {
            _today = today;
            _rand = new Random(seed);
            TestPasswordProvider = new Sha256HashString($"{_rand.Next(ulong.MinValue, ulong.MaxValue)}","T3stP@ssw0rd");

            OriginatorIdDictionary.Add($"{nameof(BasicDraftEvent1)}", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"{nameof(BasicDraftEvent2)}", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"{nameof(BasicOpenEvent1)}", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"{nameof(BasicOpenEvent2)}", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"{nameof(BasicFixedEvent)}", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"{nameof(EditorExistEvent1)}", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"{nameof(EditorExistEvent2)}", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"{nameof(EditorExistEvent3)}", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"InputUserExample0", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"InputUserExample1", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"InputUserExample2", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"InputUserExample3", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"InputUserExample4", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"InputUserExample5", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"UpdateUserExample", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"DeleteUserExample", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");
            OriginatorIdDictionary.Add($"MemberUserExample", $"{_rand.Next(ulong.MinValue, ulong.MaxValue)}");

            JustNewEvent = new EventModel();
            AllTestData.Add(JustNewEvent);

            InitialFillEvent = new EventModel()
            {
                Name = "",
                Dates = new List<DateTimeOffset>(),
                State = EventState.Draft,
                Originator = "",
                Schedules = new Dictionary<string, Schedule>(),
                Permission = SchedulePermissionToKnow.All,
                Editors = new List<string>(),
                Members = new List<string>(),
                Guild = String.Empty,
                Channel = String.Empty,
                MinEntry = 0,
                MaxEntry = int.MaxValue,
                FixDate = DateTime.Now,
                HashedPassword = ""
            };
            AllTestData.Add(InitialFillEvent);

            BasicDraftEvent1 = new EventModel()
            {
                Name = $"{nameof(BasicDraftEvent1)}",
                Dates = new List<DateTimeOffset>(),
                State = EventState.Draft,
                Originator = OriginatorIdDictionary[$"{nameof(BasicDraftEvent1)}"],
                Schedules = new Dictionary<string, Schedule>(),
                Permission = SchedulePermissionToKnow.All
            };
            AllTestData.Add(BasicDraftEvent1);

            BasicDraftEvent2 = new EventModel()
            {
                Name = $"{nameof(BasicDraftEvent2)}",
                Dates = new List<DateTimeOffset>(),
                State = EventState.Draft,
                Originator = OriginatorIdDictionary[$"{nameof(BasicDraftEvent1)}"],
                Schedules = new Dictionary<string, Schedule>(),
                Permission = SchedulePermissionToKnow.All
            };
            AllTestData.Add(BasicDraftEvent2);

            BasicOpenEvent1 = new EventModel()
            {
                Name = $"{nameof(BasicOpenEvent1)}",
                Dates = GetSampleDateTimes(3),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(BasicOpenEvent1)}"],
                Schedules = new Dictionary<string, Schedule>(),
                Permission = SchedulePermissionToKnow.All
            };
            AllTestData.Add(BasicOpenEvent1);

            BasicOpenEvent2 = new EventModel()
            {
                Name = $"{nameof(BasicOpenEvent2)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(BasicOpenEvent1)}"],
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary["InputUserExample1"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample1"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample2"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample2"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample3"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample3"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample4"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample4"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All
            };
            AllTestData.Add(BasicOpenEvent2);

            BasicFixedEvent = new EventModel()
            {
                Name = $"{nameof(BasicFixedEvent)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Fixed,
                Originator = OriginatorIdDictionary[$"{nameof(BasicFixedEvent)}"],
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary["InputUserExample2"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample2"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample3"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample3"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample4"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample4"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                FixDate = GetSampleDateTimes(5)[_rand.Next(0, 5)]
            };
            AllTestData.Add(BasicFixedEvent);

            EditorExistEvent1 = new EventModel()
            {
                Name = $"{nameof(EditorExistEvent1)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                Members = new List<string>()
                {
                    OriginatorIdDictionary["MemberUserExample"]
                }
            };
            AllTestData.Add(EditorExistEvent1);

            EditorExistEvent1P = new EventModel()
            {
                Name = $"{nameof(EditorExistEvent1P)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                HashedPassword = TestPasswordProvider.ToString(),
                Members = new List<string>()
                {
                    OriginatorIdDictionary["MemberUserExample"]
                }
            };
            AllTestData.Add(EditorExistEvent1P);

            EditorExistEvent2 = new EventModel()
            {
                Name = $"{nameof(EditorExistEvent2)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent2)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"],
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent2)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary["InputUserExample4"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample4"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
            };
            AllTestData.Add(EditorExistEvent2);

            EditorExistEvent3 = new EventModel()
            {
                Name = $"{nameof(EditorExistEvent3)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"],
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent2)}"],
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary["InputUserExample3"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample3"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample4"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample4"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All
            };
            AllTestData.Add(EditorExistEvent3);

            UpdatedEvent = new EventModel()
            {
                Name = "Updated",
                Description = "Updated Description",
                Dates = new List<DateTimeOffset>()
                {
                    _today
                },
                State = EventState.Fixed,
                Originator = "Updated",
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary["InputUserExample3"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample3"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample4"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample4"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.Originator,
                Editors = new List<string>() { "Updated" },
                Members = new List<string>() { "Updated" },
                Guild = "Updated",
                Channel = "Updated",
                MinEntry = 1,
                MaxEntry = 2,
                FixDate = _today,
                HashedPassword = "Updated"
            };

            UpdateStateTarget1 = new EventModel()
            {
                Name = $"{nameof(UpdateStateTarget1)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Draft,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                Members = new List<string>()
                {
                    OriginatorIdDictionary["MemberUserExample"]
                }
            };
            AllTestData.Add(UpdateStateTarget1);

            UpdateStateTarget1P = new EventModel()
            {
                Name = $"{nameof(UpdateStateTarget1P)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Draft,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                HashedPassword = TestPasswordProvider.ToString(),
                Members = new List<string>()
                {
                    OriginatorIdDictionary["MemberUserExample"]
                }
            };
            AllTestData.Add(UpdateStateTarget1P);

            FilterScheduleTest1 = new EventModel()
            {
                Name = $"{nameof(EditorExistEvent1P)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["MemberUserExample"], GetSampleSchedule(OriginatorIdDictionary["MemberUserExample"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample1"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample1"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                HashedPassword = TestPasswordProvider.ToString(),
                Members = new List<string>()
                {
                    OriginatorIdDictionary["MemberUserExample"]
                }
            };

            FilterScheduleTest2 = new EventModel()
            {
                Name = $"{nameof(EditorExistEvent1P)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["MemberUserExample"], GetSampleSchedule(OriginatorIdDictionary["MemberUserExample"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample1"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample1"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                HashedPassword = TestPasswordProvider.ToString(),
                Members = new List<string>()
                {
                    OriginatorIdDictionary["MemberUserExample"]
                }
            };

            FilterScheduleTest3 = new EventModel()
            {
                Name = $"{nameof(EditorExistEvent1P)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["MemberUserExample"], GetSampleSchedule(OriginatorIdDictionary["MemberUserExample"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample1"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample1"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                HashedPassword = TestPasswordProvider.ToString(),
                Members = new List<string>()
                {
                    OriginatorIdDictionary["MemberUserExample"]
                }
            };

            FilterScheduleTest4 = new EventModel()
            {
                Name = $"{nameof(EditorExistEvent1P)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["MemberUserExample"], GetSampleSchedule(OriginatorIdDictionary["MemberUserExample"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample1"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample1"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                HashedPassword = TestPasswordProvider.ToString(),
                Members = new List<string>()
                {
                    OriginatorIdDictionary["MemberUserExample"]
                }
            };

            FilterScheduleTest5 = new EventModel()
            {
                Name = $"{nameof(EditorExistEvent1P)}",
                Dates = GetSampleDateTimes(5),
                State = EventState.Open,
                Originator = OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"],
                Editors = new List<string>()
                {
                    OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"]
                },
                Schedules = new Dictionary<string, Schedule>()
                {
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent1)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleSchedule(OriginatorIdDictionary[$"{nameof(EditorExistEvent3)}"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["MemberUserExample"], GetSampleSchedule(OriginatorIdDictionary["MemberUserExample"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample1"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample1"], GetSampleDateTimes(5))},
                    {OriginatorIdDictionary["InputUserExample5"], GetSampleSchedule(OriginatorIdDictionary["InputUserExample5"], GetSampleDateTimes(5))}
                },
                Permission = SchedulePermissionToKnow.All,
                HashedPassword = TestPasswordProvider.ToString(),
                Members = new List<string>()
                {
                    OriginatorIdDictionary["MemberUserExample"]
                }
            };
        }

        public List<DateTimeOffset> GetSampleDateTimes(int num)
        {
            List<DateTimeOffset> value = new List<DateTimeOffset>();
            for (int i = 0; i < num; i++)
            {
                value.Add(_today + TimeSpan.FromDays(i));
            }

            return value;
        }

        public Schedule GetSampleSchedule(string userId, List<DateTimeOffset> dates)
        {
            var result = new Schedule()
            {
                UserId = userId,
                Name = $"Name as {userId}",
                DateStatuses = GetSampleDateStatuses(dates)
            };
            return result;
        }

        public List<DateStatus> GetSampleDateStatuses(List<DateTimeOffset> dates)
        {
            var result = new List<DateStatus>();
            dates.ForEach(date => result.Add(new DateStatus(date, (AttendanceStatus)_rand.Next(0, Enum.GetNames(typeof(AttendanceStatus)).Length))));
            return result;
        }
    }

    public static class Extends
    {
        public static ulong Next(this Random random, ulong min, ulong max)
        {
            if (max <= min)
            {
                throw new ArgumentOutOfRangeException($"{nameof(max)}", $"{nameof(max)} must be > {nameof(min)}!");
            }

            ulong uRange = max - min;
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return ulongRand % uRange + min;
        }
    }
}