# WeeklyXamarin.mobile
Mobile Application for the Weekly Xamarin Newsletter

- iOS: [![Build status](https://build.appcenter.ms/v0.1/apps/09fc316d-728a-4ff7-a71a-b45530485cc2/branches/master/badge)](https://appcenter.ms)

# Functional Requirements
* View Issues
* View Articles 
  * Inside the application (Reader view if we can)
  * Launch out to default browser
* View Contributors (curation or articles)
* Add an article (submit an article for review) - via a share target
* View events (streams, meetups, large events)
* Search articles (title, description, author, tags)
* Push Notifications (of new articles)
* Save an article
* Mark an article as a favorite
* Upvote articles
* Suggest topics to provide a backlog of ideas for content creators

# Functional - Nice To Haves
* Cache HTML of articles for offline view (maybe a user setting)

# Non-Functional Requirements
* Theming (dark / light / custom modes)
* No Auth - user not required to login
* CI/CD Github Actions
* Testing builds and releases (AppCenter)
* UITests (maybe depending on the complexity and cost)
* Caching of Newsletter Issues (Editions) & Articles (Titles, authors, tags, links)

# Goals
* Recognise the community
* No Backend - No hosting infrastructure

# Platforms
* iOS, Android (Priority 1)
* UWP (maybe via UNO), Wasm, MacOS, Linux (Nice to have)
* Blazor
* Dual Screen

# Tech
* [Xamarin.Forms](https://github.com/xamarin/Xamarin.Forms)
* Shell
* Pancakes - all the way down
* [Lottie](https://github.com/Baseflow/LottieXamarin)
* Caching - [Akavache](https://github.com/reactiveui/Akavache), [MonkeyCache](https://github.com/jamesmontemagno/monkey-cache)

Development
 * Very open source, anyone can contribute to the app
 * Contributions welcome (and acknowledged)
