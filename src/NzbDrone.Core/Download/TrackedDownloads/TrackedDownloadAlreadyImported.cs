﻿using System.Collections.Generic;
using System.Linq;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.History;

namespace NzbDrone.Core.Download.TrackedDownloads
{
    public interface ITrackedDownloadAlreadyImported
    {
        bool IsImported(TrackedDownload trackedDownload, List<EpisodeHistory> historyItems);
    }

    public class TrackedDownloadAlreadyImported : ITrackedDownloadAlreadyImported
    {
        public bool IsImported(TrackedDownload trackedDownload, List<EpisodeHistory> historyItems)
        {
            if (historyItems.Empty())
            {
                return false;
            }

            var allEpisodesImportedInHistory = trackedDownload.RemoteEpisode.Episodes.All(e =>
            {
                var lastHistoryItem = historyItems.FirstOrDefault(h => h.EpisodeId == e.Id);

                if (lastHistoryItem == null)
                {
                    return false;
                }

                return lastHistoryItem.EventType == EpisodeHistoryEventType.DownloadFolderImported;
            });

            return allEpisodesImportedInHistory;
        }
    }
}
