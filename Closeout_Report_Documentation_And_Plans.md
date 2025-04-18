# Closeout Report CS690 Final Project
## Documentation Links and Future Plans

### DOCUMENTATION & MAINTENANCE

Over the course of this project, documentation was produced for deployment of the application, and to provide guidance
for the end user who will nominally be using this application. Given the current level of completeness, there is not
much additional information to be included at this time.


### REMAINING ISSUES / ENHANCEMENT OPPORTUNITIES

- Better Login Functionality

Given the problem scope for this class, I did not get to the point of implementing full user/password functionality for
handling user login to an app -- there are many considerations that would need to be addressed going forwards in such a
lifecycle such as where the data is stored ( on device or remotely ) and given the intense personal nature of the data,
how would it need to be properly secured.

- CLI Logic Standardization -- "no magic numbers, no magic colors"

Handling the color options within Spectre Console CLI interface elements shouldn't be done on color alone, but instead
on what you're trying to CONVEY with the color -- I went with a list of colors mapped to things like 'Emphasis' and
'Query' to try and centralize a one-point-of-modification for updating or changing the colors; I'm not sure there isn't
a better way to handle it, but I'm not terribly familiar with C# at this point.

- Architecture / Refactoring Opportunities

90% of my recent work experience is split between bulk data mining/ML processing in Python notebooks, and doing real
time image processing using C++ in environments that have near-zero direct user interaction -- I think that given a long
weekend where I can read something like the 'Head First Design Patterns in C#' book, or re-read C# In Depth might be
useful if I were to restart the project.

- Platform-appropriate Storage Options

I went with JSON because it's low-hanging fruit and I understand it very well.  There are just enough things you can do
with JSON in C# using LINQ to make it a passable datastore for pretty much any application --- that being said, however,
I think a more appropriate long term goal would be to refactor the app to use something like SQLite instead, due to the
widespread availability and ubiquity of the platform and its reputation for being able to support things on embedded
systems.

- Port to a mobile framework

If our mythical end user's point of interaction with the app was always known to be a keyboard, then we'd be fine
staying within the CLI model and using Spectre Console or an equivalent --- the obvious long term porting goal would be
taking this 'minimum viable prototype' implemented in C# and leveraging something like the .NET Maui framework to jump
the gap to 'cross platform graphical application' if we needed a go-to-market strategy. If I were forced to pick another
tech stack entirely in which to rewrite the app to chase this goal, I'd probably start over with Python 3 and use PyQt
or the BeeWare libraries as my toolkit of choice -- map the project to your teams abilities, as they say.  
