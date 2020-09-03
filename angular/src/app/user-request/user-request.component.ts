import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UserRequestServiceProxy, UserRequestDtoPagedResultDto, AdminServiceProxy, AcceptUserRequestDto, UserRequestDto, ActivateUserSubscriptionDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/paged-listing-component-base';


class PagedUserRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}
@Component({
  templateUrl: './user-request.component.html',
  animations: [appModuleAnimation()]
})
export class UserRequestComponent extends PagedListingComponentBase<UserRequestDto> {
  constructor(injector: Injector,
    private _userRequestService: UserRequestServiceProxy,
    private _adminService: AdminServiceProxy,
  ) {
    super(injector);

  }

  keyword: string;
  userRequest: UserRequestDtoPagedResultDto;
  ngOnInIt() {

  }
  list(
    request: PagedUserRequestDto,
    pageNumber: number
  ): void {
    request.keyword = this.keyword;
    request.isActive = false;

    this._userRequestService
      .getPaginatedAll(
        
        request.keyword,
        request.skipCount,
        request.maxResultCount
      )
      .subscribe((result: UserRequestDtoPagedResultDto) => {
        this.userRequest = result;
        this.isTableLoading = false;
        console.log("userRequest", result);
        this.showPaging(result, pageNumber);
      });
  }

  delete() {

  }

  acceptRequest(event: UserRequestDto) {
    var acceptUserRequestDto = new AcceptUserRequestDto();
    acceptUserRequestDto.userRequestId = event.id;
    this._adminService.acceptUserRequest(acceptUserRequestDto).subscribe((result) => {
      if (result)
        this.notify.info(this.l('SavedSuccessfully'));
    })
  }

  activateUser(event: UserRequestDto) {
    var activateUserRequestDto = new ActivateUserSubscriptionDto();
    activateUserRequestDto.userId = event.userId;
    this._adminService.activateUserSubscription(activateUserRequestDto).subscribe((result) => {
      if (result)
      var inputObj = new PagedUserRequestDto();
      this.list(inputObj,1);
        this.notify.info(this.l('SavedSuccessfully'));
    })
  }
}
