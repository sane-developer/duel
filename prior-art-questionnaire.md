# Prior art questionnaire — the work system (v1 / v2 / v3)

Purpose: give Lumina an honest baseline to be measured against. The research question is
whether a different architecture can avoid the *shared-Core scoping pain* — not whether a
newer stack is nicer.

**Ground rules**

- Shapes and patterns only. No client names, no proprietary business specifics.
- Include what *works*. A summary of only the pain produces a caricature, and Lumina would
  then be designed against a strawman.
- Describe the real system, not the architecture diagram. Where it diverged from the
  intent is usually where the information is.

**Tags**

- `[P1]` answer first · `[P2]` if there's energy · `[P3]` nice to have
- `[you]` only a human knows this · `[code]` a Claude session with repo access can answer it

---

## A. Orientation

- `[P1][you]` What does the product do, in two sentences? Who sits in front of it?
- `[P1][you]` v1, v2, v3: for each — roughly when, what stack, and **why the rewrite
  happened**. What did each rewrite set out to fix?
- `[P1][you]` Did it fix it? Where did each rewrite succeed and where did the old problem
  reappear in new clothes?
- `[P1][you]` Which version does each client run? Are clients spread across v2 and v3
  simultaneously, and if so for how long has that been true?
- `[P2][you]` Is v3 intended to fully replace v2, and is that on a timeline anyone believes?

## B. Variation between clients — the core section

- `[P1][you]` What actually varies between clients? Rough taxonomy, and which dominates:
  branding and text · config values · business rules · whole features present or absent ·
  the shape of an entity · integrations with client-side systems · workflow and process
  definitions · reports · permissions.
- `[P1][you]` Rough split of your time: work that benefits all clients vs work for one
  client.
- `[P1][code]` How is client-specific behaviour expressed in code today? Per-client
  modules, feature flags, config tables, interfaces + DI, subclassing, conditionals on a
  client id, separate branches, something else?
- `[P2][you]` Is there a spread between mostly-standard clients and effectively bespoke
  ones? What does the most divergent one need that others don't?
- `[P1][you]` Can a client-specific change be made *without* touching shared Core?
  Roughly what fraction can, in practice rather than in principle?

## C. Deployment and tenancy

- `[P1][you]` One deployment per client, or one instance serving all of them?
- `[P1][code]` One database per client, or shared with a tenant discriminator?
- `[P1][you]` Are all clients on the same Core version at any given time, or can they
  diverge? Who decides when a client upgrades?
- `[P2][you]` What does a release look like — cadence, who approves, what can go wrong.

## D. Anatomy of a Core change

- `[P1][you]` **One concrete recent Core change that hurt.** Walk it through: what needed
  changing, what broke or might have broken, what you did to be sure, how long it took.
- `[P1][you]` How do you determine which clients a Core change affects? Tests, grep,
  institutional memory, asking someone?
- `[P1][you]` "Being forced to scope features" — what does that mean concretely? Who
  forces it, at what point in the work, and what's the failure it's protecting against?
- `[P2][you]` What's the worst outcome that has actually happened from a Core change?

## E. Extension mechanism

- `[P1][code]` v2 runs on Orchard, which has its own module/extension model. Is that
  system actually used to carry client variation, or is it bypassed?
- `[P1][you]` If it's bypassed — why? What did it fail to express?
- `[P2][code]` What's v3's equivalent mechanism, if it has one?
- `[P2][code]` Feature flags: roughly how many, where they live, who can toggle them, and
  whether any are permanent rather than transitional.
- `[P2][you]` Any plugin/hook/event system. Does it get used, or worked around?

## F. Permissions

- `[P1][you]` One or two of the hardest real permission rules, stated in business terms
  rather than code. Verbatim awkwardness is welcome.
- `[P1][you]` Which axes does it combine: role · per-object · organisational hierarchy ·
  data-dependent conditions · time-dependent · delegation or acting-on-behalf-of.
- `[P2][you]` Does the permission model itself vary per client, or only its data?
- `[P2][you]` Who administers permissions in practice, and do they understand the model?
- `[P3][code]` Where is a permission check physically written — one layer, or scattered?

## G. Data model

- `[P2][code]` The 5–10 central entities and how they relate.
- `[P2][you]` Which of those differ in shape per client?
- `[P2][code]` Anything schema-flexible — EAV, JSON columns, user-defined fields, dynamic
  content types. Orchard leans on this heavily; how much of it is load-bearing?

## H. What works

- `[P1][you]` What would you keep, unchanged, if you rebuilt tomorrow?
- `[P1][you]` What has the shared Core genuinely bought? Name things that would be *worse*
  with fully separate per-client codebases.
- `[P2][you]` What does onboarding a new client look like, in elapsed time and in work? Is
  that number good or bad?
- `[P2][you]` Which parts have been stable for years and never cause trouble?

## I. Scale

- `[P2][you]` Number of clients · users per client · rough data volume · team size ·
  age of each version · how much of the team touches Core.

## J. Counterfactual

- `[P1][you]` If you restarted with no constraints, what's the first thing you'd change?
- `[P1][you]` What do you believe is *impossible* in the current architecture — not merely
  hard?
- `[P2][you]` What do you suspect the v3 team believed that turned out to be wrong?
