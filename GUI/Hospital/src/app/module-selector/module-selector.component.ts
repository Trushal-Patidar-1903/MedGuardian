import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-module-selector',
  standalone: false,
  templateUrl: './module-selector.component.html',
  styleUrl: './module-selector.component.scss'
})
export class ModuleSelectorComponent {
   constructor(private router: Router) {}

   navigateToModule(path: string) {
    this.router.navigate([path]);
  }
}
