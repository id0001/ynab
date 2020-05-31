import InMemoryCache from "./InMemoryCache";
import fnv1a from "@/utilities/hashing/fnv1a";

export default class CachedFunction {
  constructor(fn, minutesToLive = 10) {
    this.cache = new InMemoryCache(minutesToLive);

    if (typeof fn !== "function") {
      throw new Error("'fn' is not a function");
    }

    this.fn = fn;
  }

  invoke(...args) {
    const key = keygen(args);

    if (this.cache.isExpired(key)) {
      const result = this.fn(...args);
      if (result && result instanceof Promise) {
        return result.then(value => {
          this.cache.set(key, value);
          return value;
        });
      } else if (result) {
        this.cache.set(key, result);
        return result;
      }
    }

    return Promise.resolve(this.cache.getValue(key));
  }

  invalidate(...args) {
    const key = keygen(args);
    this.cache.unset(key);
  }

  clear() {
    this.cache.clear();
  }
}

function keygen(...args) {
  return fnv1a(args.join(",", args.map(a => a.toString())));
}
