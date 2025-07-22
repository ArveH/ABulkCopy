DROP COLLATION IF EXISTS en_ci_ai;
CREATE COLLATION en_ci_ai (provider = 'icu', locale = 'en-u-ks-level1', deterministic = false);

DROP COLLATION IF EXISTS en_ci_ai_like;
CREATE COLLATION en_ci_ai_like (provider = 'icu',  locale = 'en-u-ks-level1',  deterministic = true);

DROP COLLATION IF EXISTS en_ci_as;
CREATE COLLATION en_ci_as (provider = 'icu', locale = 'en-u-ks-level2', deterministic = false);

DROP COLLATION IF EXISTS en_ci_as_like;
CREATE COLLATION en_ci_as_like (provider = 'icu',  locale = 'en-u-ks-level2',  deterministic = true);