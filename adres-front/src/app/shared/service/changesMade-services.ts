import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChangesMadeService {
  private changeMadeSubject = new BehaviorSubject<boolean>(false);
  change$ = this.changeMadeSubject.asObservable();

  notifyChange() {
    this.changeMadeSubject.next(true);
  }

  resetNotification() {
    this.changeMadeSubject.next(false);
  }
}
