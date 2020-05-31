export default class InMemoryCache {
  constructor(minutesToLive = 10) {
    this.millisecondsToLive = minutesToLive * 60 * 1000;
    this.cache = new Map();
  }

  set(key, value) {
    this.cache.set(key, {
      setDate: new Date(),
      value: value
    });
  }

  getExpireDate(key) {
    if (!this.cache.has(key)) {
      throw new Error("Key does not exist: " + key);
    }

    return this.cache.get(key).setDate;
  }

  getValue(key) {
    if (!this.cache.has(key)) {
      throw new Error("Key does not exist: " + key);
    }

    return this.cache.get(key).value;
  }

  isExpired(key) {
    if (!this.cache.has(key)) return true;

    return this.getExpireDate(key).getTime() + this.millisecondsToLive < new Date().getTime();
  }

  unset(key) {
    if (this.cache.has(key)) {
      this.cache.delete(key);
    }
  }

  clear() {
    this.cache.clear();
  }
}
