export class SimpleChannel {
  private _arrayBuffer: any = [];
  private _pullResolve: any = [];

  public send(data: any) {
    if (this._pullResolve.length > 0) {
      let resolve = this._pullResolve.pop();
      resolve(data);
    } else {
      this._arrayBuffer.push(data);
    }
  }

  public get() {
    return new Promise((resolve) => {
      if (this._arrayBuffer.length > 0) {
        resolve(this._arrayBuffer.pop());
      }

      this._pullResolve.push(resolve);
    });
  }
}
