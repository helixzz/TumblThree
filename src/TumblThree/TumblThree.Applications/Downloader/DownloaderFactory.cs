﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using TumblThree.Applications.Services;
using TumblThree.Domain.Models;

namespace TumblThree.Applications.Downloader
{
    [Export(typeof(IDownloaderFactory))]

    public class DownloaderFactory : IDownloaderFactory
    {
        [ImportMany(typeof(IDownloader))]
        public IEnumerable<Lazy<IDownloader, IBlogTypeMetaData>> DownloaderFactoryLazy { get; set; }


        [ImportingConstructor]
        public DownloaderFactory()
        {
        }

        public IDownloader GetDownloader(BlogTypes blogtype)
        {
            var downloaderInstance = DownloaderFactoryLazy.FirstOrDefault(list => list.Metadata.BlogType == blogtype);

            if (downloaderInstance != null)
            {
                return downloaderInstance.Value;
            }
            throw new ArgumentException("Website is not supported!", "blogType");
        }

        public IDownloader GetDownloader(BlogTypes blogtype, IShellService shellService, ICrawlerService crawlerService, IBlog blog)
        {
            switch (blogtype)
            {
                case BlogTypes.tumblr:
                    return new TumblrDownloader(shellService, crawlerService, blog);
                case BlogTypes.instagram:
                    //return new InstagramDownloader(shellService, crawlerService, blog);
                default:
                    throw new ArgumentException("Website is not supported!", "blogType");
            }
        }
    }
}
