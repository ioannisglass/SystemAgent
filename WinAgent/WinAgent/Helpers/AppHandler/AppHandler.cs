using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinAgent.Helpers
{
    public class AppHandler
    {
        /*
        private readonly AppUninstaller _appUninstaller;
        private static int RunUninstall(UninstallList list, bool isQuiet, bool isUnattended, bool isVerbose)
        {
            Console.WriteLine(@"Starting bulk uninstall...");
            var apps = QueryApps(isQuiet, isUnattended, isVerbose);

            apps = apps.Where(a => list.TestEntry(a) == true).OrderBy(x => x.DisplayName).ToList();

            if (apps.Count == 0)
            {
                Console.WriteLine(@"No applications matched the supplied uninstall list.");
                return 0;
            }

            Console.WriteLine(@"{0} application(s) were matched by the list: {1}", apps.Count,
                          string.Join("; ", apps.Select(x => x.DisplayName)));

            Console.WriteLine(@"These applications will now be uninstalled PERMANENTLY.");

            if (!isUnattended)
            {
                Console.WriteLine(@"Do you want to continue? [Y]es/[N]o");
                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                    return CancelledByUser();
            }

            Console.WriteLine(@"Setting-up for the uninstall task...");
            var targets = apps.Select(a => new BulkUninstallEntry(a, a.QuietUninstallPossible, UninstallStatus.Waiting))
                .ToList();
            var task = UninstallManager.CreateBulkUninstallTask(targets,
                new BulkUninstallConfiguration(false, isQuiet, false, true, true));
            var isDone = false;
            task.OnStatusChanged += (sender, args) =>
            {
                ClearCurrentConsoleLine();

                var running = task.AllUninstallersList.Count(x => x.IsRunning);
                var waiting = task.AllUninstallersList.Count(x => x.CurrentStatus == UninstallStatus.Waiting);
                var finished = task.AllUninstallersList.Count(x => x.Finished);
                var errors = task.AllUninstallersList.Count(x => x.CurrentStatus == UninstallStatus.Failed ||
                    x.CurrentStatus == UninstallStatus.Invalid);
                Console.Write("Running: {0}, Waiting: {1}, Finished: {2}, Failed: {3}",
                    running, waiting, finished, errors);

                if (task.Finished)
                {
                    isDone = true;
                    Console.WriteLine();
                    Console.WriteLine(@"Uninstall task Finished.");

                    foreach (var error in task.AllUninstallersList.Where(x =>
                        x.CurrentStatus != UninstallStatus.Completed && x.CurrentError != null))
                        Console.WriteLine($@"Error: {error.UninstallerEntry.DisplayName} - {error.CurrentError.Message}");
                }
            };
            task.Start();

            while (!isDone)
                Thread.Sleep(250);

            return 0;
        }

        public void RunQuietUninstall()
        {
            _appUninstaller.RunUninstall(_listView.SelectedUninstallers, _listView.AllUninstallers, true);
        }*/
    }
}
